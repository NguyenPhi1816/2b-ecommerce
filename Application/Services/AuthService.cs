using _2b_ecommerce.Application.DTOs.Auth;
using Application.Interfaces;
using Domain.Enums;
using _2b_ecommerce.Infrastructure.Models;
using _2b_ecommerce.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public sealed class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IPasswordService _passwords;
    private readonly IJwtTokenService _jwt;

    public AuthService(AppDbContext db, IPasswordService passwords, IJwtTokenService jwt)
    {
        _db = db;
        _passwords = passwords;
        _jwt = jwt;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var exists = await _db.Users.AnyAsync(u => u.Email.ToLower() == email, ct);
        if (exists)
            throw new InvalidOperationException("Email already exists.");

        var gender = ParseGender(request.Gender);

        var user = new Users
        {
            Id = Guid.NewGuid(),
            Email = email,
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Dob = request.Dob,
            Gender = gender,
            PhoneNumber = request.PhoneNumber.Trim(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        user.PasswordHash = _passwords.Hash(user, request.Password);

        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);

        return await _jwt.CreateAsync(user, ct);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email, ct);

        if (user is null)
            throw new UnauthorizedAccessException("Invalid credentials.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Account is deactivated.");

        var verification = _passwords.Verify(user, user.PasswordHash, request.Password);
        if (verification == PasswordVerificationResult.Failed)
            throw new UnauthorizedAccessException("Invalid credentials.");

        if (verification == PasswordVerificationResult.SuccessRehashNeeded)
        {
            user.PasswordHash = _passwords.Hash(user, request.Password);
            _db.Users.Update(user);
            await _db.SaveChangesAsync(ct);
        }

        return await _jwt.CreateAsync(user, ct);
    }

    private static GenderType ParseGender(string gender)
    {
        if (Enum.TryParse<GenderType>(gender, true, out var parsed))
            return parsed;

        throw new ArgumentException($"Unsupported gender: {gender}", nameof(gender));
    }
}
