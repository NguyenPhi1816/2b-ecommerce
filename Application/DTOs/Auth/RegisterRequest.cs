using System.ComponentModel.DataAnnotations;

namespace _2b_ecommerce.Application.DTOs.Auth;

public sealed class RegisterRequest
{
    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Email { get; init; } = default!;

    [Required]
    [StringLength(128, MinimumLength = 6)]
    public string Password { get; init; } = default!;

    [Required]
    [StringLength(100)]
    public string FirstName { get; init; } = default!;

    [Required]
    [StringLength(100)]
    public string LastName { get; init; } = default!;

    [DataType(DataType.Date)]
    public DateOnly Dob { get; init; }

    [Required]
    [StringLength(20)]
    public string Gender { get; init; } = default!;

    [Required]
    [Phone]
    [StringLength(20)]
    public string PhoneNumber { get; init; } = default!;
}
