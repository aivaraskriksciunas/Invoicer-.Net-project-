using Invoicer.Api.Features.ClientsFeature;
using Invoicer.Core.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Invoicer.Api.Features.BillableRecordFeature.Dto;

namespace Invoicer.Api.Features.BillableRecordFeature;

[Route("/Api/Client/{clientId}/BillableRecord")]
public class BillableRecordController : BaseApiController
{
    private readonly UserManager<User> _userManager;
    private readonly BillableRecordService _billableRecordService;
    private readonly ClientService _clientService;

    public BillableRecordController(
        UserManager<User> userManager,
        BillableRecordService billableRecordService,
        ClientService clientService )
    {
        _userManager = userManager;
        _billableRecordService = billableRecordService;
        _clientService = clientService;
    }

    [Route("")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BillableRecordDto>>> Index(
        int clientId )
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
        int clientId, 
        BillableRecordDto model )
    {
        if ( ModelState.IsValid )
        {
            var user = await _userManager.GetUserAsync( User );
            var client = await _clientService.GetByIdForUser( clientId, user );
            if ( client == null )
            {
                return NotFound();
            }

            var record = await _billableRecordService.CreateBillableRecord( model.ToBillableRecord(), client );
            return Ok( record.ToBillableRecordDto() );
        }

        return BadRequest( model );
    }
}
