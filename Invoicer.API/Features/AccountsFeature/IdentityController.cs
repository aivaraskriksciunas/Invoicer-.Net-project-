using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using Invoicer.Core.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Invoicer.Api.Features.AccountsFeature.Services;

namespace Invoicer.Api.Features.AccountsFeature;

[Route("/Api/auth")]
public class IdentityController : BaseApiController
{
    private static readonly EmailAddressAttribute _emailAddressAttribute = new();

    private IOptionsMonitor<BearerTokenOptions> bearerTokenOptions;
    private SignInManager<User> signInManager;
    private UserManager<User> userManager;
    private TimeProvider timeProvider;
    private IEmailSender<User> emailSender;
    private LinkGenerator linkGenerator;
    private ConfirmationEmailSender confirmationEmailSender;

    public IdentityController(
        IOptionsMonitor<BearerTokenOptions> bearerTokenOptions,
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        TimeProvider timeProvider,
        IEmailSender<User> emailSender,
        LinkGenerator linkGenerator,
        ConfirmationEmailSender confirmationEmailSender )
    {
        this.bearerTokenOptions = bearerTokenOptions;
        this.signInManager = signInManager;
        this.userManager = userManager;
        this.timeProvider = timeProvider;
        this.emailSender = emailSender;
        this.linkGenerator = linkGenerator;
        this.confirmationEmailSender = confirmationEmailSender;
    }

    [HttpPost]
    [Route("refresh")]
    [AllowAnonymous]
    public async Task<Results<Ok<AccessTokenResponse>, UnauthorizedHttpResult, SignInHttpResult, ChallengeHttpResult>> Refresh( 
        [FromBody] RefreshRequest refreshRequest
    )
    {
        var refreshTokenProtector = bearerTokenOptions.Get( IdentityConstants.BearerScheme ).RefreshTokenProtector;
        var refreshTicket = refreshTokenProtector.Unprotect( refreshRequest.RefreshToken );

        // Reject the /refresh attempt with a 401 if the token expired or the security stamp validation fails
        if (refreshTicket?.Properties?.ExpiresUtc is not { } 
            expiresUtc ||
            timeProvider.GetUtcNow() >= expiresUtc ||
            await signInManager.ValidateSecurityStampAsync( refreshTicket.Principal ) is not User user )
        {
            return TypedResults.Challenge();
        }

        var newPrincipal = await signInManager.CreateUserPrincipalAsync( user );
        return TypedResults.SignIn( newPrincipal, authenticationScheme: IdentityConstants.BearerScheme );
    }

    [HttpGet]
    [Route("confirmEmail", Name = ConfirmationEmailSender.ConfirmEmailEndpointName)]
    [AllowAnonymous]
    public async Task<Results<ContentHttpResult, UnauthorizedHttpResult>> ConfirmEmail( 
        [FromQuery] string userId, 
        [FromQuery] string code, 
        [FromQuery] string? changedEmail )
    {
        if ( await userManager.FindByIdAsync( userId ) is not { } user )
        {
            // We could respond with a 404 instead of a 401 like Identity UI, but that feels like unnecessary information.
            return TypedResults.Unauthorized();
        }

        try
        {
            code = Encoding.UTF8.GetString( WebEncoders.Base64UrlDecode( code ) );
        }
        catch ( FormatException )
        {
            return TypedResults.Unauthorized();
        }

        IdentityResult result;

        if ( string.IsNullOrEmpty( changedEmail ) )
        {
            result = await userManager.ConfirmEmailAsync( user, code );
        }
        else
        {
            // As with Identity UI, email and user name are one and the same. So when we update the email,
            // we need to update the user name.
            result = await userManager.ChangeEmailAsync( user, changedEmail, code );

            if ( result.Succeeded )
            {
                result = await userManager.SetUserNameAsync( user, changedEmail );
            }
        }

        if ( !result.Succeeded )
        {
            return TypedResults.Unauthorized();
        }

        return TypedResults.Text( "Thank you for confirming your email." );
    }

    [HttpPost]
    [Route( "resendConfirmationEmail" )]
    [AllowAnonymous]
    public async Task<Ok> ResendConfirmationEmail( 
        [FromBody] ResendConfirmationEmailRequest resendRequest
    )
    {
        if ( await userManager.FindByEmailAsync( resendRequest.Email ) is not { } user )
        {
            return TypedResults.Ok();
        }

        await confirmationEmailSender.SendConfirmationEmailAsync( user, HttpContext, resendRequest.Email );
        return TypedResults.Ok();
    }

    [HttpPost]
    [Route("forgotPassword")]
    [AllowAnonymous]
    public async Task<Results<Ok, ValidationProblem>> ForgotPassword( 
        [FromBody] ForgotPasswordRequest resetRequest )
    {
        var user = await userManager.FindByEmailAsync( resetRequest.Email );

        if ( user is not null && await userManager.IsEmailConfirmedAsync( user ) )
        {
            var code = await userManager.GeneratePasswordResetTokenAsync( user );
            code = WebEncoders.Base64UrlEncode( Encoding.UTF8.GetBytes( code ) );

            await emailSender.SendPasswordResetCodeAsync( user, resetRequest.Email, HtmlEncoder.Default.Encode( code ) );
        }

        // Don't reveal that the user does not exist or is not confirmed, so don't return a 200 if we would have
        // returned a 400 for an invalid code given a valid user email.
        return TypedResults.Ok();
    }

    [HttpPost]
    [Route("resetPassword")]
    [AllowAnonymous]
    public async Task<Results<Ok, ValidationProblem>> ResetPassword( 
        [FromBody] ResetPasswordRequest resetRequest )
    {
        var user = await userManager.FindByEmailAsync( resetRequest.Email );

        if ( user is null || !(await userManager.IsEmailConfirmedAsync( user )) )
        {
            // Don't reveal that the user does not exist or is not confirmed, so don't return a 200 if we would have
            // returned a 400 for an invalid code given a valid user email.
            return CreateValidationProblem( IdentityResult.Failed( userManager.ErrorDescriber.InvalidToken() ) );
        }

        IdentityResult result;
        try
        {
            var code = Encoding.UTF8.GetString( WebEncoders.Base64UrlDecode( resetRequest.ResetCode ) );
            result = await userManager.ResetPasswordAsync( user, code, resetRequest.NewPassword );
        }
        catch ( FormatException )
        {
            result = IdentityResult.Failed( userManager.ErrorDescriber.InvalidToken() );
        }

        if ( !result.Succeeded )
        {
            return CreateValidationProblem( result );
        }

        return TypedResults.Ok();
    }

    [HttpPost]
    [Route("manage/2fa")]
    [Authorize]
    public async Task<Results<Ok<TwoFactorResponse>, ValidationProblem, NotFound>> Manage2fa( 
        [FromBody] TwoFactorRequest tfaRequest )
    {
        var userManager = signInManager.UserManager;
        if ( await userManager.GetUserAsync( User ) is not { } user )
        {
            return TypedResults.NotFound();
        }

        if ( tfaRequest.Enable == true )
        {
            if ( tfaRequest.ResetSharedKey )
            {
                return CreateValidationProblem( "CannotResetSharedKeyAndEnable",
                    "Resetting the 2fa shared key must disable 2fa until a 2fa token based on the new shared key is validated." );
            }
            else if ( string.IsNullOrEmpty( tfaRequest.TwoFactorCode ) )
            {
                return CreateValidationProblem( "RequiresTwoFactor",
                    "No 2fa token was provided by the request. A valid 2fa token is required to enable 2fa." );
            }
            else if ( !await userManager.VerifyTwoFactorTokenAsync( user, userManager.Options.Tokens.AuthenticatorTokenProvider, tfaRequest.TwoFactorCode ) )
            {
                return CreateValidationProblem( "InvalidTwoFactorCode",
                    "The 2fa token provided by the request was invalid. A valid 2fa token is required to enable 2fa." );
            }

            await userManager.SetTwoFactorEnabledAsync( user, true );
        }
        else if ( tfaRequest.Enable == false || tfaRequest.ResetSharedKey )
        {
            await userManager.SetTwoFactorEnabledAsync( user, false );
        }

        if ( tfaRequest.ResetSharedKey )
        {
            await userManager.ResetAuthenticatorKeyAsync( user );
        }

        string[]? recoveryCodes = null;
        if ( tfaRequest.ResetRecoveryCodes || (tfaRequest.Enable == true && await userManager.CountRecoveryCodesAsync( user ) == 0) )
        {
            var recoveryCodesEnumerable = await userManager.GenerateNewTwoFactorRecoveryCodesAsync( user, 10 );
            recoveryCodes = recoveryCodesEnumerable?.ToArray();
        }

        if ( tfaRequest.ForgetMachine )
        {
            await signInManager.ForgetTwoFactorClientAsync();
        }

        var key = await userManager.GetAuthenticatorKeyAsync( user );
        if ( string.IsNullOrEmpty( key ) )
        {
            await userManager.ResetAuthenticatorKeyAsync( user );
            key = await userManager.GetAuthenticatorKeyAsync( user );

            if ( string.IsNullOrEmpty( key ) )
            {
                throw new NotSupportedException( "The user manager must produce an authenticator key after reset." );
            }
        }

        return TypedResults.Ok( new TwoFactorResponse
        {
            SharedKey = key,
            RecoveryCodes = recoveryCodes,
            RecoveryCodesLeft = recoveryCodes?.Length ?? await userManager.CountRecoveryCodesAsync( user ),
            IsTwoFactorEnabled = await userManager.GetTwoFactorEnabledAsync( user ),
            IsMachineRemembered = await signInManager.IsTwoFactorClientRememberedAsync( user ),
        } );
    }

    [HttpGet]
    [Route("manage/info")]
    [Authorize]
    public async Task<Results<Ok<InfoResponse>, ValidationProblem, NotFound>> GetAccountInfo()
    {
        if ( await userManager.GetUserAsync( User ) is not { } user )
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok( await CreateInfoResponseAsync( user, userManager ) );
    }

    [HttpPost]
    [Route("manage/info")]
    [Authorize]
    public async Task<Results<Ok<InfoResponse>, ValidationProblem, NotFound>> ChangeAccountInfo(
        [FromBody] InfoRequest infoRequest
    )
    {
        if ( await userManager.GetUserAsync( User ) is not { } user )
        {
            return TypedResults.NotFound();
        }

        if ( !string.IsNullOrEmpty( infoRequest.NewEmail ) && !_emailAddressAttribute.IsValid( infoRequest.NewEmail ) )
        {
            return CreateValidationProblem( IdentityResult.Failed( userManager.ErrorDescriber.InvalidEmail( infoRequest.NewEmail ) ) );
        }

        if ( !string.IsNullOrEmpty( infoRequest.NewPassword ) )
        {
            if ( string.IsNullOrEmpty( infoRequest.OldPassword ) )
            {
                return CreateValidationProblem( "OldPasswordRequired",
                    "The old password is required to set a new password. If the old password is forgotten, use /resetPassword." );
            }

            var changePasswordResult = await userManager.ChangePasswordAsync( user, infoRequest.OldPassword, infoRequest.NewPassword );
            if ( !changePasswordResult.Succeeded )
            {
                return CreateValidationProblem( changePasswordResult );
            }
        }

        if ( !string.IsNullOrEmpty( infoRequest.NewEmail ) )
        {
            var email = await userManager.GetEmailAsync( user );

            if ( email != infoRequest.NewEmail )
            {
                await confirmationEmailSender.SendConfirmationEmailAsync( user, HttpContext, infoRequest.NewEmail, isChange: true );
            }
        }

        return TypedResults.Ok( await CreateInfoResponseAsync( user, userManager ) );
    }

    private static ValidationProblem CreateValidationProblem( string errorCode, string errorDescription )
    {
        return TypedResults.ValidationProblem( new Dictionary<string, string[]> {
            { errorCode, [errorDescription] }
            } );
    }

    private static ValidationProblem CreateValidationProblem( IdentityResult result )
    {
        // We expect a single error code and description in the normal case.
        // This could be golfed with GroupBy and ToDictionary, but perf! :P
        Debug.Assert( !result.Succeeded );
        var errorDictionary = new Dictionary<string, string[]>( 1 );

        foreach ( var error in result.Errors )
        {
            string[] newDescriptions;

            if ( errorDictionary.TryGetValue( error.Code, out var descriptions ) )
            {
                newDescriptions = new string[descriptions.Length + 1];
                Array.Copy( descriptions, newDescriptions, descriptions.Length );
                newDescriptions[descriptions.Length] = error.Description;
            }
            else
            {
                newDescriptions = [error.Description];
            }

            errorDictionary[error.Code] = newDescriptions;
        }

        return TypedResults.ValidationProblem( errorDictionary );
    }
    private static async Task<InfoResponse> CreateInfoResponseAsync<TUser>( TUser user, UserManager<TUser> userManager )
        where TUser : class
    {
        return new()
        {
            Email = await userManager.GetEmailAsync( user ) ?? throw new NotSupportedException( "Users must have an email." ),
            IsEmailConfirmed = await userManager.IsEmailConfirmedAsync( user ),
        };
    }

    [AttributeUsage( AttributeTargets.Parameter )]
    private sealed class FromBodyAttribute : Attribute, IFromBodyMetadata
    {
    }

    [AttributeUsage( AttributeTargets.Parameter )]
    private sealed class FromServicesAttribute : Attribute, IFromServiceMetadata
    {
    }

    [AttributeUsage( AttributeTargets.Parameter )]
    private sealed class FromQueryAttribute : Attribute, IFromQueryMetadata
    {
        public string? Name => null;
    }
}