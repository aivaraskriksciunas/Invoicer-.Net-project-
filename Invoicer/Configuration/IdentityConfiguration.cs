using System;
using Invoicer.Data.Models;
using Invoicer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Invoicer.Configuration;

public static class IdentityConfiguration
{
    public static void AddInvoicerIdentity( this IServiceCollection services )
    {
        services.AddDefaultIdentity<User>( options => options.SignIn.RequireConfirmedAccount = false )
            .AddRoles<IdentityRole>()
            .AddApiEndpoints()
            .AddEntityFrameworkStores<InvoicerDbContext>();
        services.AddRazorPages();

        // Ensure all users are authenticated by default
        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        services.Configure<IdentityOptions>(options =>
        {
            // Password settings.
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 5;
            options.Password.RequiredUniqueChars = 0;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;
        });
    }

    public static async Task AddAdminUserAsync( this IApplicationBuilder app )
    {
        using ( var scope = app.ApplicationServices.CreateScope() )
        {
            var user = new User{ UserName = "admin@test.com", Email = "admin@test.com", EmailConfirmed = true };
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            
            if ( ! await roleManager.RoleExistsAsync( User.Roles.Admin ) ) {
                await roleManager.CreateAsync( new IdentityRole( User.Roles.Admin ) );
            }
            
            var usersInRole = await userManager.GetUsersInRoleAsync( User.Roles.Admin );
            if ( !usersInRole.Any() ) 
            {
                await userManager.CreateAsync( user, "admin" );
                await userManager.AddToRoleAsync( user, User.Roles.Admin );
            }
        }
    }
}
