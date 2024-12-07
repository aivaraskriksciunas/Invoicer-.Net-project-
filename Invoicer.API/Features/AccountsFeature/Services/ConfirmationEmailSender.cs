using Invoicer.Core.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;

namespace Invoicer.Api.Features.AccountsFeature.Services;

public class ConfirmationEmailSender
{
    private UserManager<User> userManager;
    private LinkGenerator linkGenerator;
    private IEmailSender<User> emailSender;

    public const string ConfirmEmailEndpointName = "ConfirmEmailEndpoint";

    public ConfirmationEmailSender(
        UserManager<User> userManager, 
        LinkGenerator linkGenerator,
        IEmailSender<User> emailSender )
    {
        this.userManager = userManager;
        this.linkGenerator = linkGenerator;
        this.emailSender = emailSender;
    }

    public async Task SendConfirmationEmailAsync( User user, HttpContext context, string email, bool isChange = false )
    {
        var code = isChange
            ? await userManager.GenerateChangeEmailTokenAsync( user, email )
            : await userManager.GenerateEmailConfirmationTokenAsync( user );
        code = WebEncoders.Base64UrlEncode( Encoding.UTF8.GetBytes( code ) );

        var userId = await userManager.GetUserIdAsync( user );
        var routeValues = new RouteValueDictionary()
        {
            ["userId"] = userId,
            ["code"] = code,
        };

        if ( isChange )
        {
            // This is validated by the /confirmEmail endpoint on change.
            routeValues.Add( "changedEmail", email );
        }

        var confirmEmailUrl = linkGenerator.GetUriByName( context, ConfirmEmailEndpointName, routeValues )
            ?? throw new NotSupportedException( $"Could not find endpoint named '{ConfirmEmailEndpointName}'." );

        await emailSender.SendConfirmationLinkAsync( user, email, HtmlEncoder.Default.Encode( confirmEmailUrl ) );
    }
}
