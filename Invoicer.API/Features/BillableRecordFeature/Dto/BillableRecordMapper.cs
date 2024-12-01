﻿using Invoicer.Core.Data.Models;
using Riok.Mapperly.Abstractions;

namespace Invoicer.Api.Features.BillableRecordFeature.Dto;

[Mapper]
public static partial class BillableRecordMapper
{
    public static partial BillableRecordDto ToBillableRecordDto( this BillableRecord record );

    public static partial IEnumerable<BillableRecordDto> ToBillableRecordDto( this IEnumerable<BillableRecord> list );
}
