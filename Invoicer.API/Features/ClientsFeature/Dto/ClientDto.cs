using Invoicer.Core.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Invoicer.Api.Features.ClientsFeature.Dto;

public record ClientDto
{
    public string Id { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [MinLength(4)]
    [MaxLength(20)]
    public string PhoneNumber { get; set; }

    [MaxLength(200)]
    public string AddressLine1 { get; set; }

    [MaxLength(200)]
    public string AddressLine2 { get; set; }

    [Required]
    public string UserId { get; set; }
}
