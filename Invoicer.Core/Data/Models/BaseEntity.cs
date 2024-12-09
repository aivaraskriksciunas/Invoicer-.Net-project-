using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoicer.Core.Data.Models;

public abstract class BaseEntity : IEntity
{
    [MaxLength(32)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }
}
