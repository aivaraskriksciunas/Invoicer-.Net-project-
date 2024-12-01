using System.ComponentModel.DataAnnotations;

namespace Invoicer.Api.Features.BillableRecordFeature.Dto;

public record BillableRecordDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(255)]
    public string Name { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }
}
