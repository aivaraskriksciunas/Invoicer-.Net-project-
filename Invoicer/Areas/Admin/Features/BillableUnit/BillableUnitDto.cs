using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Invoicer.Data.Models;

namespace Invoicer.Areas.Admin.Features.BillableUnit;

public class BillableUnitDto
{
    public int? Id { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(50)]
    [DisplayName("Full name")]
    public required string FullName { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(10)]
    [DisplayName("Abbreviation")]
    public required string ShortName { get; set; }

    [DisplayName("Allow whole values only")]
    public required bool WholeValuesOnly { get; set; } = false;

    public Data.Models.BillableUnit ToBillableUnit()
    {
        return new Data.Models.BillableUnit {
            ShortName = ShortName,
            FullName = FullName,
            WholeValuesOnly = WholeValuesOnly
        };
    }

    public static BillableUnitDto FromBillableUnit( Data.Models.BillableUnit billableUnit ) 
    {
        return new BillableUnitDto {
            Id = billableUnit.Id,
            ShortName = billableUnit.ShortName,
            FullName = billableUnit.FullName,
            WholeValuesOnly = billableUnit.WholeValuesOnly
        };
    }
}
