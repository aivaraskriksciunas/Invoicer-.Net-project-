using System;
using Invoicer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Areas.Admin.Features.BillableUnit;

public class BillableUnitController : BaseAdminController
{
    private readonly InvoicerDbContext _db;

    public BillableUnitController( InvoicerDbContext dbContext )
    {
        _db = dbContext;
    }

    public async Task<IActionResult> Index()
    {
        var units = await _db.BillableUnits.ToListAsync();

        return View( units );
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [Bind] BillableUnitDto model
    )
    {
        if ( ModelState.IsValid )
        {
            try {
                await _db.BillableUnits.AddAsync( model.ToBillableUnit() );
                await _db.SaveChangesAsync();

                return Redirect( nameof( Index) );
            } catch {
                ModelState.AddModelError( "", "There was an error saving the unit" );
            }
        }

        return View( model );
    }

    public async Task<IActionResult> Edit( int id )
    {
        var unit = await _db.BillableUnits.FindAsync( id );
        if ( unit == null ) {
            return NotFound();
        }

        return View( BillableUnitDto.FromBillableUnit( unit ) );
    }

    [HttpPost]
    public async Task<IActionResult> Edit( int id, [Bind]BillableUnitDto model )
    {
        if ( ! await _db.BillableUnits.Where( u => u.Id == id ).AnyAsync() ) {
            return NotFound();
        }

        if ( ModelState.IsValid ) 
        {
            var dbmodel = model.ToBillableUnit();
            dbmodel.Id = id;

            try {
                _db.BillableUnits.Update( dbmodel );
                await _db.SaveChangesAsync();
                return RedirectToAction( nameof( Index) );
            }
            catch {
                ModelState.AddModelError( "", "There was an error saving the changes." );
            }
        }

        return View( model );
    }
}
