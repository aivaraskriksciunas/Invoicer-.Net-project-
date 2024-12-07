using Bogus;
using Invoicer.Core.Data;
using Invoicer.Core.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Respawn;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Invoicer.Tests.Features;

[Collection( nameof( ApiTestCollection ) )]
public abstract class ApiTest : IAsyncLifetime
{

    protected readonly InvoicerApplicationFactory factory;

    protected readonly Faker faker;


    protected UserManager<User> userManager
    {
        get => factory.UserManager;
    }

    protected InvoicerDbContext db
    {
        get => factory.Db;
    }

    protected User? authenticatedUser;

    protected ApiTest( InvoicerApplicationFactory factory )
    {
        this.factory = factory;
        this.faker = new Faker();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await factory.ResetDatabase();
    }

    protected HttpContent SerializeJson( object data )
    {
        return new StringContent( JsonSerializer.Serialize( data ), Encoding.UTF8, "application/json" );
    }

    protected async Task<User> CreateUser( string email = "admin@test.com", string password = "admin" )
    {
        var user = new User { Email = email, UserName = email, EmailConfirmed = true };
        var res = await userManager.CreateAsync( user, "admin" );
        if ( !res.Succeeded )
        {
            throw new InvalidDataException( res.ToString() );
        }

        return user;
    }

    protected async Task<HttpClient> CreateClientWithAuth( string email = "admin@test.com", string password = "admin" )
    {
        return CreateClientWithAuth( await CreateUser( email, password ) );
    }

    protected HttpClient CreateClientWithAuth( User authenticatedUser )
    {
        this.authenticatedUser = authenticatedUser;

        var testfactory = factory.WithWebHostBuilder( builder =>
        {
            builder.ConfigureTestServices( services =>
            {
                services.AddAuthentication( defaultScheme: "TestScheme" )
                    .AddScheme<TestAuthenticationSchemeOptions, TestAuthHandler>(
                        "TestScheme", options => {
                            options.DefaultUser = authenticatedUser;
                        } );
            } );
        } );

        return testfactory.CreateClient();
    }

    public class TestAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public User? DefaultUser { get; set; }
    }

    public class TestAuthHandler : AuthenticationHandler<TestAuthenticationSchemeOptions>
    {
        public TestAuthHandler( IOptionsMonitor<TestAuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder )
            : base( options, logger, encoder )
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if ( Options.DefaultUser == null )
            {
                return Task.FromResult(
                    AuthenticateResult.Fail( new UnauthorizedAccessException() )
                );
            }

            var claims = new[] { 
                new Claim( ClaimTypes.Email, Options.DefaultUser.Email ),
                new Claim( ClaimTypes.Name, Options.DefaultUser.UserName ),
                new Claim( ClaimTypes.NameIdentifier, Options.DefaultUser.Id ),
            };
            var identity = new ClaimsIdentity( claims, "TestScheme" );
            var principal = new ClaimsPrincipal( identity );
            var ticket = new AuthenticationTicket( principal, "TestScheme" );

            return Task.FromResult( AuthenticateResult.Success( ticket ) );
        }
    }
}
