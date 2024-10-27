using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Invoicer.Core.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Invoicer.Tests.Features.AccountsFeature;

public class AccountsTests : ApiTest
{
    public AccountsTests( 
        InvoicerApplicationFactory factory )
        : base( factory )
    {}

    [Fact]
    public async Task Post_Login_UserCanLogin()
    {
        // Arrange
        var client = factory.CreateClient();
        await CreateUser( "admin@test.com", "admin" );

        var loginRequest = new
        {
            Email = "admin@test.com",
            Password = "admin"
        };
        var content = new StringContent( JsonSerializer.Serialize( loginRequest ), Encoding.UTF8, "application/json" );

        // Act
        var response = await client.PostAsync( "/login", content );

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
    }

    [Fact]
    public async Task Post_Login_InvalidUserCannotLogin()
    {
        // Arrange
        var client = factory.CreateClient();
        var loginRequest = new
        {
            Email = "invalid@test.com",
            Password = "admin"
        };

        var content = new StringContent( JsonSerializer.Serialize( loginRequest ), Encoding.UTF8, "application/json" );

        // Act
        var response = await client.PostAsync( "/login", content );

        // Assert
        Assert.Equal( HttpStatusCode.Unauthorized, response.StatusCode ); // Status Code 200-299
    }

    [Fact]
    public async Task Post_UserCanGetInformationAfterLogin()
    {
        // Arrange
        var client = await CreateClientWithAuth( "admin@test.com", "admin" );

        // Act
        var response = await client.GetAsync( "/Api/User" );

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains( "admin@test.com", responseBody );
    }
}
