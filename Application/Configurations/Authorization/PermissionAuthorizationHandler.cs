using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using _2b_ecommerce.Infrastructure.Persistence;

namespace Application.Configurations.Authorization;

public sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly AppDbContext _db;

    public PermissionAuthorizationHandler(AppDbContext db) => _db = db;

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        if (context.User.HasClaim("perm", requirement.Permission))
        {
            context.Succeed(requirement);
            return;
        }

        var subject = context.User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                     ?? context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(subject, out var userId))
            return;

        var userRole = await _db.Users
            .AsNoTracking()
            .Where(u => u.Id == userId && u.IsActive)
            .Select(u => new { u.RoleId })
            .FirstOrDefaultAsync();

        if (userRole?.RoleId is not Guid roleId)
            return;

        var hasPermission = await _db.RolePermissions
            .AsNoTracking()
            .Where(rp => rp.RoleId == roleId)
            .AnyAsync(rp => rp.Permission.Resource + ":" + rp.Permission.Action == requirement.Permission);

        if (hasPermission)
            context.Succeed(requirement);
    }
}
