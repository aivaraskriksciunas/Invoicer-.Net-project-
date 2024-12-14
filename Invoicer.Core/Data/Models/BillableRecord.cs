using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoicer.Core.Data.Models;

public class BillableRecord : BaseEntity
{
    [Required]
    [MaxLength(300)]
    public required string Name { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    [Required]
    public string ClientId { get; set; }
    public Client? Client { get; set; }

    [Required]
    [Column(TypeName = "decimal(10, 4)")]
    public double Amount { get; set; } = 0;

    [Required]
    public string? BillableUnitId { get; set; }
    public BillableUnit? BillableUnit { get; set; }

    [Required]
    [Column(TypeName = "decimal(10, 4)")]
    public double PricePerUnit { get; set; } = 0;
}
