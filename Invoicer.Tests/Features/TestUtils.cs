using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoicer.Tests.Features;

public static class TestUtils
{
    public static bool DateTimeEquals( DateTime? original, DateTime? compare )
    {
        if ( original == null && compare == null ) return true;
        if ( original == null || compare == null ) return false;

        var format = "yyyy-MM-dd HH:mm:ss";
        var compareStr = compare.Value
            .ToUniversalTime()
            .ToString( format );

        return original.Value
            .ToUniversalTime()
            .ToString( format )
            .Equals( compareStr );
    }
}
