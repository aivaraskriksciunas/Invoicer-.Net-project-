using System;
using System.ComponentModel.DataAnnotations;

namespace Invoicer.Data.Models;

public class Client : IEntity
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string PhoneNumber { get; set; }

    public string AddressLine1 { get; set; }

    public string AddressLine2 { get; set; }

    public ICollection<BillableRecord> BillableRecords { get; set;} = new List<BillableRecord>();
}
