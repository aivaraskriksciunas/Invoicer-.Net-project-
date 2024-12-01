using Bogus;
using Invoicer.Core.Data.Models;
using System.Net.Http.Json;

namespace Invoicer.Tests.Features.BillableRecordFeature;

public class BillableRecordTests : ApiTest
{
    public BillableRecordTests(
        InvoicerApplicationFactory factory )
        : base( factory )
    { }

    [Fact]
    public async void Get_ShouldGetClientBillableRecords()
    {
        // Arrange
        var user = await CreateUser();
        var httpclient = CreateClientWithAuth( user );

        var client = CreateClientFaker( user ).Generate();
        await db.AddAsync( client );
        await db.SaveChangesAsync();

        var records = CreateBillableRecordFaker( client ).Generate( 5 );
        await db.AddRangeAsync( records );
        await db.SaveChangesAsync();

        // Act 
        var response = await httpclient.GetAsync( $"/Api/Client/{client.Id}/BillableRecord" );

        // Assert 
        response.EnsureSuccessStatusCode();
        var res = await response.Content.ReadFromJsonAsync<List<BillableRecord>>();
        Assert.NotNull( res );
        Assert.Equal( 5, res.Count );
    }

    [Fact]
    public async void Get_ShouldGetBillableRecordsForSpecifiedClient()
    {
        // Arrange
        var user = await CreateUser();
        var httpclient = CreateClientWithAuth( user );

        var client = CreateClientFaker( user ).Generate();
        var client2 = CreateClientFaker( user ).Generate();
        await db.AddAsync( client );
        await db.AddAsync( client2 );
        await db.SaveChangesAsync();

        var records = CreateBillableRecordFaker( client ).Generate( 3 );
        var records2 = CreateBillableRecordFaker( client2 ).Generate( 3 );
        await db.AddRangeAsync( records );
        await db.AddRangeAsync( records2 );
        await db.SaveChangesAsync();

        // Act 
        var response = await httpclient.GetAsync( $"/Api/Client/{client.Id}/BillableRecord" );

        // Assert 
        response.EnsureSuccessStatusCode();
        var res = await response.Content.ReadFromJsonAsync<List<BillableRecord>>();
        Assert.NotNull( res );
        Assert.Equal( 3, res.Count );

        foreach ( var badrecord in records2 )
        {
            // Ensure none of the records from different client show up
            Assert.Null( res.Find( r => r.Id == badrecord.Id ) );
        }
    }

    [Fact]
    public async void Get_ShouldNotGetRecordsFromNotOwnClient()
    {
        // Arrange
        var user = await CreateUser();
        var owner = await CreateUser( "test@test.com" );
        var httpclient = CreateClientWithAuth( user );

        var client = CreateClientFaker( owner ).Generate();
        await db.AddAsync( client );
        await db.SaveChangesAsync();

        var records = CreateBillableRecordFaker( client ).Generate( 3 );
        await db.AddRangeAsync( records );
        await db.SaveChangesAsync();

        // Act 
        var response = await httpclient.GetAsync( $"/Api/Client/{client.Id}/BillableRecord" );

        // Assert 
        Assert.Equal( System.Net.HttpStatusCode.NotFound, response.StatusCode );
    }

    private Faker<Client> CreateClientFaker( User user )
    {
        return new Faker<Client>()
            .RuleFor( c => c.Name, f => f.Company.CompanyName() )
            .RuleFor( c => c.AddressLine1, f => f.Address.FullAddress() )
            .RuleFor( c => c.UserId, _ => user.Id );
    }

    private Faker<BillableRecord> CreateBillableRecordFaker( Client client )
    {
        return new Faker<BillableRecord>()
            .RuleFor( c => c.Name, f => f.Lorem.Sentence( 3 ) )
            .RuleFor( c => c.ClientId, _ => client.Id )
            .RuleFor( c => c.StartTime, f => f.Date.Recent( 30 ).ToUniversalTime() )
            .RuleFor(
                c => c.EndTime,
                ( f, current ) => f.PickRandomParam<DateTime?>(
                    null,
                    current.StartTime.AddHours( f.Random.Number( 1, 48 ) ).ToUniversalTime()
                )
            );
    }
}
