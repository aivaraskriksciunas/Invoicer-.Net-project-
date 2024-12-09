using System;
using System.ComponentModel.DataAnnotations;

namespace Invoicer.Core.Data.Models;

public class Client : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [MaxLength(500)]
    public string? AddressLine1 { get; set; }

    [MaxLength(500)]
    public string? AddressLine2 { get; set; }

    public string UserId { get; set; }
    public User User { get; set; }

    public ICollection<BillableRecord> BillableRecords { get; set;} = new List<BillableRecord>();
}
