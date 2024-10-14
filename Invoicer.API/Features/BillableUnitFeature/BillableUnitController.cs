using Invoicer.Core.Data;
using Invoicer.Core.Data.Models;
using Invoicer.Core.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Api.Features.BillableUnitFeature;


public class BillableUnitController : BaseApiController
{
    private readonly IRepository<BillableUnit> _repository;
    private readonly InvoicerDbContext _db;

    public BillableUnitController( 
        IRepository<BillableUnit> repository,
        InvoicerDbContext db )
    {
        _repository = repository;
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BillableUnitDto>>> Index()
    {
        var res = await _db.BillableUnits.ToDto().ToListAsync();
        return Ok( res );
    }
}
