using Invoicer.Api.Features.AccountsFeature.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Invoicer.Core.Data.Models;
using Invoicer.Api.Features.AccountsFeature.Services;

namespace Invoicer.Api.Features.AccountsFeature;

public class RegisterController : BaseApiController
{
    private UserManager<User> userManager;
    private IUserStore<User> userStore;
    private ConfirmationEmailSender confirmationEmailSender;

    public RegisterController(
        UserManager<User> userManager,
        IUserStore<User> userStore,
        ConfirmationEmailSender confirmationEmailSender )
    {
        this.userManager = userManager;
        this.userStore = userStore;
        this.confirmationEmailSender = confirmationEmailSender;
    }

    [Route( "/register" )]
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<UserDto>> Register(
        [FromBody] RegisterUserDto registration
    )
    {
        if ( registration.Password != registration.PasswordConfirm )
        {
            ModelState.AddModelError( nameof( registration.Password ), "Passwords do not match" );
        }

        if ( ModelState.IsValid )
        {
            var user = new User
            {
                Email = registration.Email,
                UserName = registration.Email,
                FirstName = registration.FirstName,
                LastName = registration.LastName,
            };

            var emailStore = (IUserEmailStore<User>)userStore;
            var result = await userManager.CreateAsync( user, registration.Password );

            if ( !result.Succeeded )
            {
                return BadRequest( result );
            }

            await confirmationEmailSender.SendConfirmationEmailAsync( 
                user, 
                HttpContext, 
                user.Email );
            return Ok();
        }

        return BadRequest( registration );
    }
}
