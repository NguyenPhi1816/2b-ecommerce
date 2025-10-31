using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using _2b_ecommerce.Application.DTOs.Auth;
using _2b_ecommerce.Application.Configurations.Options;
using _2b_ecommerce.Infrastructure.Models;
using _2b_ecommerce.Infrastructure.Persistence;
using Microsoft.IdentityModel.Tokens;
using Application.Interfaces;

namespace Application.Services;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly JwtOptions _opt;
    private readonly AppDbContext _db;

    public JwtTokenService(IOptions<JwtOptions> opt, AppDbContext db)
    {
        _opt = opt.Value;
        _db = db;
    }

    public async Task<AuthResponse> CreateAsync(Users user, CancellationToken ct = default)
    {
        var roleCode = await _db.Roles
            .Where(r => r.Id == user.RoleId)
            .Select(r => r.Code)
            .FirstOrDefaultAsync(ct);

        var permissions = await _db.RolePermissions
            .Where(rp => rp.RoleId == user.RoleId)
            .Include(rp => rp.Permission)
            .Select(rp => rp.Permission.Resource + ":" + rp.Permission.Action)
            .ToListAsync(ct);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("name", $"{user.FirstName} {user.LastName}"),
        };

        if (!string.IsNullOrWhiteSpace(roleCode))
            claims.Add(new Claim(ClaimTypes.Role, roleCode));

        // nhúng permissions (có thể nhiều claim trùng type "perm")
        claims.AddRange(permissions.Select(p => new Claim("perm", p)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(_opt.AccessTokenMinutes);

        var token = new JwtSecurityToken(
            issuer: _opt.Issuer,
            audience: _opt.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expires,
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return new AuthResponse
        {
            AccessToken = jwt,
            ExpiresAt = expires,
            Email = user.Email,
            Role = roleCode,
            Permissions = permissions
        };
    }
}
