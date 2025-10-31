using _2b_ecommerce.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces;

public interface IPasswordService
{
    string Hash(Users user, string raw);
    PasswordVerificationResult Verify(Users user, string hash, string raw);
}
