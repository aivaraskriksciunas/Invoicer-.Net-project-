using System;
using Invoicer.Data;
using Invoicer.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Areas.Admin.Features.BillableUnit;

public class BillableUnitController : BaseAdminController
{
    private readonly InvoicerDbContext _db;
    private readonly IRepository<Data.Models.BillableUnit> _repository;

    public BillableUnitController( 
        InvoicerDbContext db,
        IRepository<Data.Models.BillableUnit> repository 
    )
    {
        _db = db;
        _repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        var units = await _repository.FindAllAsync();

        return View( units );
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [Bind] BillableUnitViewModel model
    )
    {
        if ( ModelState.IsValid )
        {
            try {
                await _repository.CreateAsync( model.ToBillableUnit() );
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
        var unit = await _repository.FindByIdAsync( id );
        if ( unit == null ) {
            return NotFound();
        }

        return View( BillableUnitViewModel.FromBillableUnit( unit ) );
    }

    [HttpPost]
    public async Task<IActionResult> Edit( int id, [Bind]BillableUnitViewModel model )
    {
        if ( ! await _repository.ExistsAsync( id ) ) {
            return NotFound();
        }

        if ( ModelState.IsValid ) 
        {
            var dbmodel = model.ToBillableUnit();
            dbmodel.Id = id;

            try {
                await _repository.UpdateAsync( dbmodel );
                await _db.SaveChangesAsync();
                return RedirectToAction( nameof( Index ) );
            }
            catch {
                ModelState.AddModelError( "", "There was an error saving the changes." );
            }
        }

        return View( model );
    }

    [HttpGet]
    public async Task<IActionResult> Delete( int id )
    {
        var unit = await _repository.FindByIdAsync( id );
        if ( unit == null )
        {
            return NotFound();
        }

        return View( unit );
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirm( int id )
    {
        var unit = await _repository.FindByIdAsync( id );
        if (unit == null)
        {
            return NotFound();
        }

        await _repository.DeleteAsync( id );
        await _db.SaveChangesAsync();
        return RedirectToAction( nameof( Index ) );
    }
}
