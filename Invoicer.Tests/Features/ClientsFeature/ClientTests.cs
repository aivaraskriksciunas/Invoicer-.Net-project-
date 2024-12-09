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
using Bogus;
using System.Net.Http.Json;

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
        var content = SerializeJson( new
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

    [Fact]
    public async void Get_ShouldShowOnlyUsersClients()
    {
        // Arrange
        var user1 = await CreateUser( "other@other.com", "test" );
        var user2 = await CreateUser( "admin@admin.com", "test" );

        var clientFaker = new Faker<Client>()
            .RuleFor( c => c.Name, f => f.Company.CompanyName() )
            .RuleFor( c => c.PhoneNumber, f => f.Phone.PhoneNumber() )
            .RuleFor( c => c.AddressLine1, f => f.Address.FullAddress() );

        var user1Clients = clientFaker.RuleFor( c => c.User, _ => user1 ).Generate( 1 );
        var user2Clients = clientFaker.RuleFor( c => c.User, _ => user2 ).Generate( 3 );

        await db.Clients.AddRangeAsync( user1Clients );
        await db.Clients.AddRangeAsync( user2Clients );
        await db.SaveChangesAsync();

        var client = CreateClientWithAuth( user2 );

        // Act
        var response = await client.GetAsync( "/Api/Client" );

        // Assert 
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<List<Client>>();
        Assert.Equal( 3, body.Count );
    }

    [Fact]
    public async void Put_CanUpdateClientData()
    {
        // Arrange
        var user = await CreateUser();

        var client = new Client()
        {
            User = user,
            Name = "Old value",
            PhoneNumber = "1234567890",
            AddressLine1 = "Old value",
            AddressLine2 = "Old value 2"
        };

        await _repository.CreateAsync( client );
        await db.SaveChangesAsync();

        var updatedClientData = new
        {
            Name = "New value",
            PhoneNumber = "000000000",
            AddressLine1 = "New value",
            AddressLine2 = "New value 2"
        };

        var httpclient = CreateClientWithAuth( user );

        // Act 
        var response = await httpclient.PutAsync( $"/Api/Client/{client.Id}", SerializeJson( updatedClientData ) );
        db.ChangeTracker.Clear();

        // Assert 
        response.EnsureSuccessStatusCode();

        var dbclient = await _repository.FindByIdAsync( client.Id );
        Assert.NotNull( dbclient );
        Assert.Equal( updatedClientData.Name, dbclient.Name );
        Assert.Equal( updatedClientData.PhoneNumber, dbclient.PhoneNumber );
        Assert.Equal( updatedClientData.AddressLine1, dbclient.AddressLine1 );
        Assert.Equal( updatedClientData.AddressLine2, dbclient.AddressLine2 );
    }

    [Fact]
    public async void Put_CannotUpdateOtherData()
    {
        // Arrange
        var user = await CreateUser();
        var owner = await CreateUser( "owner@owner.com" );

        var client = new Client()
        {
            User = owner,
            Name = "Old value",
            PhoneNumber = "1234567890",
            AddressLine1 = "Old value",
            AddressLine2 = "Old value 2"
        };

        await _repository.CreateAsync( client );
        await db.SaveChangesAsync();

        var updatedClientData = new
        {
            Name = "New value",
            PhoneNumber = "000000000",
            AddressLine1 = "New value",
            AddressLine2 = "New value 2"
        };

        var httpclient = CreateClientWithAuth( user );

        // Act 
        var response = await httpclient.PutAsync( $"/Api/Client/{client.Id}", SerializeJson( updatedClientData ) );

        // Assert 
        Assert.Equal( HttpStatusCode.NotFound, response.StatusCode );

        var dbclient = await _repository.FindByIdAsync( client.Id );
        Assert.NotNull( dbclient );
        Assert.Equal( client.Name, dbclient.Name );
        Assert.Equal( client.PhoneNumber, dbclient.PhoneNumber );
        Assert.Equal( client.AddressLine1, dbclient.AddressLine1 );
        Assert.Equal( client.AddressLine2, dbclient.AddressLine2 );
    }
}
