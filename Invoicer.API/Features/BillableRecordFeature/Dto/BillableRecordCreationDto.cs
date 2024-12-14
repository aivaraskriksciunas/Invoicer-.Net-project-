using Invoicer.Core.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoicer.Api.Features.BillableRecordFeature.Dto;

public record BillableRecordCreationDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(255)]
    public string Name { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    [Required]
    public double Amount { get; set; } = 0;

    [Required]
    public string BillableUnitId { get; set; }

    [Required]
    public double PricePerUnit { get; set; } = 0;
}
