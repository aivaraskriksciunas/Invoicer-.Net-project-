using System;
using System.ComponentModel.DataAnnotations;

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
}
