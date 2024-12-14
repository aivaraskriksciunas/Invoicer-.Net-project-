using System.ComponentModel.DataAnnotations;

namespace Invoicer.Api.Features.BillableRecordFeature.Dto;

public class BillableRecordDto
{
    public string Name { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public double Amount { get; set; } = 0;

    //[Required]
    //public string? BillableUnitId { get; set; }

    public double PricePerUnit { get; set; } = 0;
}
