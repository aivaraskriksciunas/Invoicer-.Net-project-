using Invoicer.Api.Features.BillableUnitFeature.Dto;
using Invoicer.Core.Data;
using Invoicer.Core.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Api.Features.BillableUnitFeature;

public class BillableUnitService
{
    private readonly InvoicerDbContext _db;

    public BillableUnitService(
        InvoicerDbContext db )
    {
        _db = db;
    }

    public async Task<BillableUnit?> GetByIdForUser( string id, User user )
    {
        return await _db.BillableUnits.Where(
            u => u.Id == id && u.UserId == user.Id
        ).FirstOrDefaultAsync();
    }

    public async Task<BillableUnit?> CreateForUser(
        BillableUnitCreationDto billableUnit, 
        User user )
    {
        var entity = billableUnit.ToBillableUnit();
        entity.User = user;

        await _db.BillableUnits.AddAsync( entity );
        await _db.SaveChangesAsync();
        return entity;
    }
}
