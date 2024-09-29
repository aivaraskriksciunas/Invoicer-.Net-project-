using System;
using System.ComponentModel.DataAnnotations;

namespace Invoicer.Data.Models;

public class BillableRecord
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(300)]
    public string Name { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    [Required]
    public Client client { get; set; }
}
