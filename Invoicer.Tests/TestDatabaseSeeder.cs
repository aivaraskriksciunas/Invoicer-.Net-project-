using Invoicer.Core.Data.Models;
using Invoicer.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Invoicer.Tests;

class TestDatabaseSeeder : DatabaseSeeder
{
    public TestDatabaseSeeder(
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager )
        : base( userManager, roleManager )
    { }

    protected override Task EnsureSuperuserExists()
    {
        return Task.CompletedTask;
    }
}
