using System;
using System.ComponentModel.DataAnnotations;

namespace Invoicer.Data.Models;

public class BillableUnit
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public required string FullName { get; set; }

    [Required]
    [MaxLength(10)]
    public required string ShortName { get; set; }

    public bool WholeValuesOnly { get; set; } = false;

    // Stores the user that created this unit
    public User? user { get; set; }
}
