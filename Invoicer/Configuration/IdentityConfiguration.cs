using System;
using Invoicer.Core.Data.Models;
using Invoicer.Core.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Invoicer.Data;

namespace Invoicer.Configuration;

public static class IdentityConfiguration
{
    public static void AddInvoicerIdentity( this IServiceCollection services )
    {
        // Register admin user seeder 
        services.AddTransient<IDatabaseSeeder, DatabaseSeeder>();

        services.AddIdentityApiEndpoints<User>( options => options.SignIn.RequireConfirmedAccount = false )
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<InvoicerDbContext>();

        services.AddRazorPages();

        services.AddAuthentication().AddCookie();

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
}
