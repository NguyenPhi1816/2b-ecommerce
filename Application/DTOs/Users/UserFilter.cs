using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Users;

public sealed class UserFilter
{
    [StringLength(100)]
    public string? Search { get; init; }
    public bool? IsActive { get; init; }
}
