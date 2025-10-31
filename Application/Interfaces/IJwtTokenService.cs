using _2b_ecommerce.Application.DTOs.Auth;
using _2b_ecommerce.Infrastructure.Models;

namespace Application.Interfaces;

public interface IJwtTokenService
{
    Task<AuthResponse> CreateAsync(Users user, CancellationToken ct = default);
}
