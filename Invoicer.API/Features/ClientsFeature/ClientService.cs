using Invoicer.Core.Data.Models;
using Invoicer.Core.Data.Repositories;

namespace Invoicer.Api.Features.ClientsFeature;

public class ClientService
{
    private IRepository<Client> _repository;

    public ClientService(
        IRepository<Client> repository    
    )
    {
        _repository = repository;
    }

    public async Task<Client?> GetByIdForUser( int id, User? user )
    {
        var client = await _repository.FindByIdAsync( id );
        if ( client == null )
        {
            return null;
        }

        if ( user == null || client.UserId != user.Id )
        {
            return null;
        }

        return client;
    }
}
