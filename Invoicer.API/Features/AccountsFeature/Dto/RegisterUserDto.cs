using System.ComponentModel.DataAnnotations;

namespace Invoicer.Api.Features.AccountsFeature.Dto;

public class RegisterUserDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(3)]
    public string FirstName { get; set; }

    public string? LastName { get; set; }

    [Required]
    [MinLength(5)]
    public string Password { get; set; }

    [Required]
    public string PasswordConfirm { get; set; }


}
