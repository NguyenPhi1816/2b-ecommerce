namespace _2b_ecommerce.Application.DTOs.Auth;

public sealed class AuthResponse
{
    public string AccessToken { get; init; } = default!;
    public DateTime ExpiresAt { get; init; }
    public string Email { get; init; } = default!;
    public string? Role { get; init; }
    public IEnumerable<string> Permissions { get; init; } = default!;
}
