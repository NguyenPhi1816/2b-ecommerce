// Services/PasswordService.cs
using Application.Interfaces;
using _2b_ecommerce.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public sealed class PasswordService : IPasswordService
{
    private readonly PasswordHasher<Users> _hasher = new();
    public string Hash(Users user, string raw) => _hasher.HashPassword(user, raw);
    public PasswordVerificationResult Verify(Users user, string hash, string raw)
        => _hasher.VerifyHashedPassword(user, hash, raw);
}
