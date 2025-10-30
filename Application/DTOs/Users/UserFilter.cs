namespace Application.DTOs.Users;

public sealed class UserFilter
{
    public string? Search { get; init; }
    public bool? IsActive { get; init; }
}
