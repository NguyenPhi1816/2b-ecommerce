namespace _2b_ecommerce.Application.DTOs.Auth;

public sealed class RegisterRequest
{
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public DateOnly Dob { get; init; }
    public string Gender { get; init; } = default!;
    public string PhoneNumber { get; init; } = default!;
}
