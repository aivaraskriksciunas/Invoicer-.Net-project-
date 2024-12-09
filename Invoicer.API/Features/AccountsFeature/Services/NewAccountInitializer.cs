using Invoicer.Core.Data;
using Invoicer.Core.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Api.Features.AccountsFeature.Services;

public class NewAccountInitializer
{
    private InvoicerDbContext _context;

    public NewAccountInitializer(
        InvoicerDbContext context)
    {
        _context = context;
    }

    public async Task InitializeNewAccount( User user )
    {
        await CreateDefaultBillableUnits( user );

        await _context.SaveChangesAsync();
    }

    private async Task CreateDefaultBillableUnits( User user )
    {
        var units = await _context.BillableUnits.Where(
            u => u.UserId == null
        ).ToListAsync();

        foreach ( var unit in units )
        {
            var userUnit = new BillableUnit
            {
                User = user,
                ShortName = unit.ShortName,
                FullName = unit.FullName,
                WholeValuesOnly = unit.WholeValuesOnly,
            };

            _context.BillableUnits.Add( userUnit );
        }
    }

}
