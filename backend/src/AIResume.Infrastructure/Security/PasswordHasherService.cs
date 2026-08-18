using AIResume.Application.Users.Interfaces;
using AIResume.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AIResume.Infrastructure.Security;

public class PasswordHasherService : IPasswordHasherService
{
    private readonly PasswordHasher<User> _passwordHasher = new();

    public string HashPassword(string password)
    {
        var temporaryUser = new User(
            "Temporary",
            "User",
            "temporary@example.com",
            string.Empty);

        return _passwordHasher.HashPassword(
            temporaryUser,
            password);
    }

    public bool VerifyPassword(
        string passwordHash,
        string providedPassword)
    {
        var temporaryUser = new User(
            "Temporary",
            "User",
            "temporary@example.com",
            passwordHash);

        var result = _passwordHasher.VerifyHashedPassword(
            temporaryUser,
            passwordHash,
            providedPassword);

        return result != PasswordVerificationResult.Failed;
    }
}