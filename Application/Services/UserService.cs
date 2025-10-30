using Application.DTOs.Pagination;
using Application.DTOs.Users;
using Application.Interfaces;
using _2b_ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

// alias cho rõ: entity scaffold là "Users" (số nhiều)
using DbUser = _2b_ecommerce.Infrastructure.Models.Users;

namespace Application.Services;

public sealed class UserService : IUserService
{
    private readonly AppDbContext _db;
    public UserService(AppDbContext db) => _db = db;

    public async Task<PagedResult<UserDto>> GetUsersAsync(UserFilter filter, int page, int pageSize, CancellationToken ct = default)
    {
        var q = _db.Set<DbUser>().AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            q = q.Where(u =>
                EF.Functions.ILike(u.Email!, $"%{term}%") ||
                EF.Functions.ILike(u.FirstName!, $"%{term}%") ||
                EF.Functions.ILike(u.LastName!, $"%{term}%") ||
                EF.Functions.ILike(u.PhoneNumber!, $"%{term}%")
            );
        }

        if (filter.IsActive.HasValue)
            q = q.Where(u => u.IsActive == filter.IsActive.Value);

        var total = await q.CountAsync(ct);

        var items = await q
            .OrderByDescending(u => u.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserDto
            {
                Id          = u.Id,
                Email       = u.Email,
                FirstName   = u.FirstName,
                LastName    = u.LastName,
                PhoneNumber = u.PhoneNumber,
                IsActive    = u.IsActive,
                RoleId      = u.RoleId,
                CreatedAt   = u.CreatedAt
            })
            .ToListAsync(ct);

        return new PagedResult<UserDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            Total = total
        };
    }

    public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _db.Set<DbUser>().AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => new UserDto
            {
                Id          = u.Id,
                Email       = u.Email,
                FirstName   = u.FirstName,
                LastName    = u.LastName,
                PhoneNumber = u.PhoneNumber,
                IsActive    = u.IsActive,
                RoleId      = u.RoleId,
                CreatedAt   = u.CreatedAt
            })
            .FirstOrDefaultAsync(ct);
    }
}
