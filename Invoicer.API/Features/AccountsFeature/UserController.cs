namespace Invoicer.Api.Features.AccountsFeature;

using Invoicer.Api.Features;
using Invoicer.Api.Features.AccountsFeature.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Invoicer.Core.Data.Models;
using System.Security.Claims;

public class UserController : BaseApiController
{
    private UserManager<User> _userManager;

    public UserController( UserManager<User> userManager )
    {
        _userManager = userManager;
    }

    /// <summary>
    /// Gets information about the currently logged in user
    /// </summary>
    /// <returns></returns>
    /// <response code="200">Returns current user information</response>
    /// <response code="401">If the user is not logged in</response>
    [Route("")]
    [HttpGet]
    [ProducesResponseType( StatusCodes.Status200OK )]
    [ProducesResponseType( StatusCodes.Status401Unauthorized )]
    [Produces( "application/json" )]
    public async Task<ActionResult<AccountDto>> getUser()
    {
        User? user = null;

        if ( User.Identity?.IsAuthenticated != null )
        {
            user = await _userManager.GetUserAsync( User );
        }

        if ( user != null )
        {
            return Ok( user.ToAccountDto() );
        }

        return Unauthorized();
    }
}
