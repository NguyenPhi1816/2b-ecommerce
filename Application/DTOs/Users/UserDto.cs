namespace Application.DTOs.Users;

public sealed class UserDto
{
    public Guid Id { get; init; }
    public string Email { get; init; } = default!;
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string? PhoneNumber { get; init; }
    public bool IsActive { get; init; }
    public Guid? RoleId { get; init; }
    public DateTime CreatedAt { get; init; }   // scaffold ra DateTime
}
