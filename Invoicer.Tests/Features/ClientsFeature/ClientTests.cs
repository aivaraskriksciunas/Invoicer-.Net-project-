using Invoicer.Core.Data.Models;
using Invoicer.Core.Data.Repositories;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Tests.Features.ClientsFeature;

public class ClientTests : ApiTest
{
    private readonly IRepository<Client> _repository;

    public ClientTests(
        InvoicerApplicationFactory appfactory )
        : base( appfactory )
    {
        _repository = appfactory.ServiceProvider.GetRequiredService<IRepository<Client>>();
    }

    [Fact]
    public async void Post_ShouldCreateClient()
    {
        // Arrange 
        var client = await CreateClientWithAuth();
        var content = JsonEncode( new
        {
            Name = "Test"
        } );

        var user = await userManager.Users.ToListAsync();

        // Act 
        var result = await client.PostAsync( "/Api/Client", content );

        // Assert 
        Assert.Equal( HttpStatusCode.Created, result.StatusCode );
        Assert.Single( await _repository.FindAllAsync() );
    }
}
