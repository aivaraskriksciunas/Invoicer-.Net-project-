using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Invoicer.Api.Features.BillableUnitFeature.Dto;

public class BillableUnitDto
{
    public string? Id { get; set; }

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
}
