using Bogus;
using Invoicer.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoicer.Tests.Fakers;

public class BillableUnitFaker : Faker<BillableUnit>
{
    public BillableUnitFaker()
    {
        RuleFor( u => u.ShortName, f => f.Lorem.Letter( f.Random.Number( 1, 2 ) ) );
        RuleFor( u => u.FullName, f => f.Lorem.Word() );
        RuleFor( u => u.WholeValuesOnly, f => f.PickRandomParam( true, false ) );
    }
}
