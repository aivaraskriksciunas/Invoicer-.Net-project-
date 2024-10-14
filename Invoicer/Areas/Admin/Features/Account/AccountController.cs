using System;
using Invoicer.Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Areas.Admin.Features.Account;

public class AccountController : BaseAdminController
{
    private InvoicerDbContext _db;

    public AccountController(
        InvoicerDbContext context
    )
    {
        _db = context;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _db.Users
            .OrderBy( p => p.UserName )
            .ToListAsync();
            
        return View( result );
    }
}
