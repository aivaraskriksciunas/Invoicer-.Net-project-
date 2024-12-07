using System;
using Microsoft.AspNetCore.Identity;

namespace Invoicer.Core.Data.Models;

public class User : IdentityUser
{
    public static class Roles {
        public const string Admin = "Admin";
    }

    private string? firstName;
    public string? FirstName {
        get => firstName; 
        set
        {
            firstName = value;
            if ( value != null && value.Length > 0 )
            {
                firstName = value[0].ToString().ToUpper() + value.Substring( 1 ).ToLower();
            }
        }
    }
    
    private string? lastName;
    public string? LastName {
        get => lastName;
        set
        {
            lastName = value;
            if ( value != null && value.Length > 0 )
            {
                lastName = value[0].ToString().ToUpper() + value.Substring( 1 ).ToLower();
            }
        }
    }

    public ICollection<Client> Clients { get; set; } = new List<Client>();


    public string GetFullName()
    {
        if ( FirstName != null && LastName != null ) 
        {
            return $"{FirstName} {LastName}";
        }

        return FirstName ?? Email ?? "";
    }
}
