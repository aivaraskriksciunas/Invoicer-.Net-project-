using Invoicer.Api.Features.ClientsFeature.Dto;
using Invoicer.Core.Data;
using Invoicer.Core.Data.Models;
using Invoicer.Core.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Invoicer.Api.Features.ClientsFeature;

public class ClientController : BaseApiController
{
    private readonly IRepository<Client> _clientRepository;
    private readonly UserManager<User> _userManager;
    private readonly InvoicerDbContext _dbContext;

    public ClientController(
        UserManager<User> userManager,
        IRepository<Client> repository,
        InvoicerDbContext context )
    {
        _clientRepository = repository;
        _userManager = userManager;
        _dbContext = context;
    }

    [Route("")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientDto>>> Index()
    {
        var id = _userManager.GetUserId( User );

        var userClients = await _clientRepository.Query.Where(
            client => client.UserId.ToString() == id )
            .ToDto()
            .ToListAsync();

        return Ok( userClients );
    }

    [Route("{id}")]
    [HttpGet]
    public async Task<ActionResult<ClientDto>> Get( int id )
    {
        var client = await _clientRepository.FindByIdAsync( id );
        if (client == null)
        {
            return NotFound();
        }

        return Ok( client );
    }

    [Route("")]
    [HttpPost]
    public async Task<ActionResult<ClientDto>> Create( [Bind]CreateClient model )
    {
        if ( ModelState.IsValid )
        {
            Client client = model.ToModel();
            var userId = _userManager.GetUserId( User );

            client.UserId = userId;

            await _clientRepository.CreateAsync( client );
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction( nameof( Get ), new { id = client.Id }, client );
        }

        return BadRequest( model );
    }
}
