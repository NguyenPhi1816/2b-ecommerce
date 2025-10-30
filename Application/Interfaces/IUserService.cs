using Application.DTOs.Pagination;
using Application.DTOs.Users;

namespace Application.Interfaces;

public interface IUserService
{
    Task<PagedResult<UserDto>> GetUsersAsync(UserFilter filter, int page, int pageSize, CancellationToken ct = default);
    Task<UserDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
}
