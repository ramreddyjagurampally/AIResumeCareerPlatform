using AIResume.Domain.Entities;

namespace AIResume.Application.Users.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}