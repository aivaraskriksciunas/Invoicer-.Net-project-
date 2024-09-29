using System;
using Microsoft.AspNetCore.Identity;

namespace Invoicer.Data.Models;

public class User : IdentityUser
{
    public static class Roles {
        public const string Admin = "Admin";
    }
}
