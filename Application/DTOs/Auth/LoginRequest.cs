using System.ComponentModel.DataAnnotations;

namespace _2b_ecommerce.Application.DTOs.Auth;

public sealed class LoginRequest
{
    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Email { get; init; } = default!;

    [Required]
    [StringLength(128, MinimumLength = 6)]
    public string Password { get; init; } = default!;
}
