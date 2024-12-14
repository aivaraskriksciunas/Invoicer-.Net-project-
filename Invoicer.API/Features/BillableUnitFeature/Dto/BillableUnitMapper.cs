using Invoicer.Api.Features.BillableUnitFeature.Dto;
using Invoicer.Core.Data.Models;
using Riok.Mapperly.Abstractions;

namespace Invoicer.Api.Features.BillableUnitFeature;

[Mapper]
public static partial class BillableUnitMapper
{
    public static partial BillableUnit ToBillableUnit( this BillableUnitDto billableUnitDto );
    public static partial BillableUnit ToBillableUnit( this BillableUnitCreationDto billableUnitCreationDto );

    public static partial BillableUnitDto ToBillableUnitDto( this BillableUnit entity );

    public static partial IQueryable<BillableUnitDto> ToDto( this IQueryable<BillableUnit> q );
}
