using Bogus;
using Invoicer.Core.Data.Models;

namespace Invoicer.Tests.Fakers;

public class ClientFaker : Faker<Client>
{
    public ClientFaker()
    {
        SetupRules();
    }

    public ClientFaker( User user )
    {
        SetupRules();
        RuleFor( c => c.UserId, _ => user.Id );
    }

    private void SetupRules()
    {
        RuleFor( c => c.Name, f => f.Company.CompanyName() );
        RuleFor( c => c.AddressLine1, f => f.Address.FullAddress() );
    }
}
