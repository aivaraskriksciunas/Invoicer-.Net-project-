using Invoicer.Api.Features.ClientsFeature;
using Invoicer.Core.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Invoicer.Api.Features.BillableRecordFeature.Dto;
using Invoicer.Api.Features.BillableUnitFeature;

namespace Invoicer.Api.Features.BillableRecordFeature;

[Route("/Api/Client/{clientId}/BillableRecord")]
public class BillableRecordController : BaseApiController
{
    private readonly UserManager<User> _userManager;
    private readonly BillableRecordService _billableRecordService;
    private readonly BillableUnitService _billableUnitService;
    private readonly ClientService _clientService;

    public BillableRecordController(
        UserManager<User> userManager,
        BillableRecordService billableRecordService,
        BillableUnitService billableUnitService,
        ClientService clientService )
    {
        _userManager = userManager;
        _billableRecordService = billableRecordService;
        _billableUnitService = billableUnitService;
        _clientService = clientService;
    }

    [Route("")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BillableRecordDto>>> Index(
        string clientId )
    {
        var user = await _userManager.GetUserAsync( User );
        var client = await _clientService.GetByIdForUser( clientId, user );
        if ( client == null )
        {
            return NotFound();
        }

        var records = await _billableRecordService.GetBillableRecordsForClient( client );
        return Ok( records.ToBillableRecordDto() );
    }

    [Route("")]
    [HttpPost]
    public async Task<ActionResult<BillableRecordDto>> Create(
        string clientId, 
        BillableRecordCreationDto model )
    {
        var user = await _userManager.GetUserAsync( User );
        var client = await _clientService.GetByIdForUser( clientId, user );
        if ( client == null )
        {
            return NotFound();
        }

        if ( ModelState.IsValid )
        {
            var unit = await _billableUnitService.GetByIdForUser( model.BillableUnitId, user );
            if ( unit == null )
            {
                ModelState.AddModelError( nameof( model.BillableUnitId ), "Selected unit does not exist" );
                return BadRequest( model );
            }

            var record = await _billableRecordService.CreateBillableRecord( model, client, unit );
            if ( record == null )
            {
                return StatusCode( 500 );
            }

            return Ok( record.ToBillableRecordDto() );
        }

        return BadRequest( model );
    }
}
