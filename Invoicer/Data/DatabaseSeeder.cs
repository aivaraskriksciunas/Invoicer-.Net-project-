using Invoicer.Core.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Invoicer.Data;

public class DatabaseSeeder : IDatabaseSeeder
{
    private UserManager<User> _userManager;
    private RoleManager<IdentityRole> _roleManager;

    public DatabaseSeeder(
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public virtual async Task SeedData()
    {
        await EnsureRolesExist();

        await EnsureSuperuserExists(); 
    }

    protected virtual async Task EnsureRolesExist()
    {
        var roles = new List<string>() { User.Roles.Admin };

        foreach ( var role in roles )
        {
            if (!await _roleManager.RoleExistsAsync( role ))
            {
                await _roleManager.CreateAsync( new IdentityRole( role ) );
            }
        }
    }

    protected virtual async Task EnsureSuperuserExists()
    {
        var user = new User { UserName = "admin@test.com", Email = "admin@test.com", EmailConfirmed = true };

        var usersInRole = await _userManager.GetUsersInRoleAsync( User.Roles.Admin );
        if ( !usersInRole.Any() )
        {
            await _userManager.CreateAsync( user, "admin" );
            await _userManager.AddToRoleAsync( user, User.Roles.Admin );
        }
    }
}
