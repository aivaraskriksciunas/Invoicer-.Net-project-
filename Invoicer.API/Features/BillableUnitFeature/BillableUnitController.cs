using Invoicer.Api.Features.BillableUnitFeature.Dto;
using Invoicer.Core.Data;
using Invoicer.Core.Data.Models;
using Invoicer.Core.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Invoicer.Api.Features.BillableUnitFeature;


public class BillableUnitController : BaseApiController
{
    private readonly IRepository<BillableUnit> _repository;
    private readonly BillableUnitService _service;
    private readonly InvoicerDbContext _db;
    private readonly UserManager<User> _userManager;

    public BillableUnitController( 
        IRepository<BillableUnit> repository,
        BillableUnitService service,
        InvoicerDbContext db,
        UserManager<User> userManager )
    {
        _repository = repository;
        _db = db;
        _userManager = userManager;
        _service = service;
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

    [HttpPost]
    [Route("")]
    public async Task<ActionResult<BillableUnitDto>> Create(
        [FromBody] BillableUnitCreationDto model )
    {
        var userId = _userManager.GetUserId( User );
        var user = await _userManager.FindByIdAsync( userId! );
        if ( user == null )
        { 
            return Unauthorized();
        }

        if ( ModelState.IsValid )
        {
            var unit = await _service.CreateForUser( model, user );
            if ( unit == null )
            {
                ModelState.AddModelError( "", "Could not create a billable unit due to an error." );
                return BadRequest( model );
            }

            return Ok( unit.ToBillableUnitDto() );
        }

        return BadRequest( model );
    }
}
