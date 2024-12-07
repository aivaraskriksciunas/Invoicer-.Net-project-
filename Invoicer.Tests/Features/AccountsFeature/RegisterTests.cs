using Bogus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Invoicer.Tests.Features.AccountsFeature;

public class RegisterTests : ApiTest
{
    public RegisterTests( 
        InvoicerApplicationFactory factory )
        : base( factory )
    {}

    [Fact]
    public async Task Post_UserCanRegister()
    {
        // Arrange
        var client = factory.CreateClient();

        var registerRequest = new
        {
            Email = "test@test.com",
            FirstName = faker.Person.FirstName,
            LastName = faker.Person.LastName,
            Password = "secret",
            PasswordConfirm = "secret"
        };
        var content = SerializeJson( registerRequest );

        // Act
        var response = await client.PostAsync( "/register", content );

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal( 1, await db.Users.CountAsync() );
        var createdUser = await db.Users.FirstAsync();
        Assert.NotNull( createdUser );
        Assert.Equal( registerRequest.Email, createdUser.Email );
        Assert.Equal( registerRequest.FirstName, createdUser.FirstName );
        Assert.Equal( registerRequest.LastName, createdUser.LastName );
    }

    [Fact]
    public async Task Post_EnsurePasswordsMatch()
    {
        // Arrange
        var client = factory.CreateClient();

        var registerRequest = new
        {
            Email = "test@test.com",
            FirstName = faker.Person.FirstName,
            LastName = faker.Person.LastName,
            Password = "secret",
            PasswordConfirm = "admin123"
        };
        var content = SerializeJson( registerRequest );

        // Act
        var response = await client.PostAsync( "/register", content );

        // Assert
        Assert.Equal( HttpStatusCode.BadRequest, response.StatusCode );
        Assert.Equal( 0, await db.Users.CountAsync() );
    }

    [Fact]
    public async Task Post_EnsureEmailIsUnique()
    {
        // Arrange
        var email = faker.Person.Email;
        await CreateUser( email, "admin" );
        var client = factory.CreateClient();

        var registerRequest = new
        {
            Email = email,
            FirstName = faker.Person.FirstName,
            LastName = faker.Person.LastName,
            Password = "secret",
            PasswordConfirm = "admin123"
        };
        var content = SerializeJson( registerRequest );

        // Act
        var response = await client.PostAsync( "/register", content );

        // Assert
        Assert.Equal( HttpStatusCode.BadRequest, response.StatusCode );
        Assert.Equal( 1, await db.Users.CountAsync() );
    }
}
