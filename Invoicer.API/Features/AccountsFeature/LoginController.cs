using Invoicer.Core.Data.Models;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Invoicer.Api.Features.AccountsFeature;

public class LoginController : BaseApiController
{

    [HttpPost]
    [Route("/login")]
    [AllowAnonymous]
    public async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>> Login( 
        [FromBody] LoginRequest login, 
        [FromQuery] bool? useCookies, 
        [FromQuery] bool? useSessionCookies, 
        [FromServices] IServiceProvider sp 
    )
    {
        var signInManager = sp.GetRequiredService<SignInManager<User>>();

        var useCookieScheme = (useCookies == true) || (useSessionCookies == true);
        var isPersistent = (useCookies == true) && (useSessionCookies != true);
        signInManager.AuthenticationScheme = useCookieScheme? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;

        var result = await signInManager.PasswordSignInAsync( login.Email, login.Password, isPersistent, lockoutOnFailure: true );

        if (result.RequiresTwoFactor )
        {
            if ( !string.IsNullOrEmpty( login.TwoFactorCode ) )
            {
                result = await signInManager.TwoFactorAuthenticatorSignInAsync( login.TwoFactorCode, isPersistent, rememberClient: isPersistent );
            }
            else if ( !string.IsNullOrEmpty( login.TwoFactorRecoveryCode ) )
            {
                result = await signInManager.TwoFactorRecoveryCodeSignInAsync( login.TwoFactorRecoveryCode );
            }
        }

        if ( !result.Succeeded )
        {
            return TypedResults.Problem( result.ToString(), statusCode: StatusCodes.Status401Unauthorized );
        }

        // The signInManager already produced the needed response in the form of a cookie or bearer token.
        return TypedResults.Empty;
    }
}
