using Bogus;
using Invoicer.Core.Data.Models;
using Invoicer.Tests.Fakers;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace Invoicer.Tests.Features.BillableRecordFeature;

public class BillableUnitTests : ApiTest
{
    public BillableUnitTests(
        InvoicerApplicationFactory factory )
        : base( factory )
    { }

    [Fact]
    public async void Get_ShouldGetClientBillableRecords()
    {
        // Arrange
        var user = await CreateUser();
        var user2 = await CreateUser( "test2@test.com" );
        var httpclient = CreateClientWithAuth( user );

        var unitFaker = new BillableUnitFaker();
        var user1Units = unitFaker.RuleFor( u => u.User, user ).Generate( 3 );
        var user2Units = unitFaker.RuleFor( u => u.User, user2 ).Generate( 3 );
        var defaultUnits = unitFaker.RuleFor( u => u.User, _ => null ).Generate( 3 );
        await db.AddRangeAsync( user1Units );
        await db.AddRangeAsync( user2Units );
        await db.AddRangeAsync( defaultUnits );
        await db.SaveChangesAsync();

        // Act 
        var response = await httpclient.GetAsync( $"/Api/BillableUnit" );

        // Assert 
        response.EnsureSuccessStatusCode();
        var res = await response.Content.ReadFromJsonAsync<List<BillableUnit>>();
        Assert.NotNull( res );
        Assert.Equal( 3, res.Count );
    }
}
