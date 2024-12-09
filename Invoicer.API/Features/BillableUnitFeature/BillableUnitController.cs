using Invoicer.Api.Features.BillableUnitFeature.Dto;
using Invoicer.Core.Data;
using Invoicer.Core.Data.Models;
using Invoicer.Core.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Api.Features.BillableUnitFeature;


public class BillableUnitController : BaseApiController
{
    private readonly IRepository<BillableUnit> _repository;
    private readonly InvoicerDbContext _db;
    private readonly UserManager<User> _userManager;

    public BillableUnitController( 
        IRepository<BillableUnit> repository,
        InvoicerDbContext db,
        UserManager<User> userManager )
    {
        _repository = repository;
        _db = db;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BillableUnitDto>>> Index()
    {
        var id = _userManager.GetUserId( User );
        var res = await _db.BillableUnits.Where(
            u => u.UserId == id
        ).ToDto().ToListAsync();

        return Ok( res );
    }
}
