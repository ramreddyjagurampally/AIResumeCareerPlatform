using AIResume.Application.Users.DTOs;
using AIResume.Application.Users.Interfaces;

namespace AIResume.Application.Users.Services;

public class UserRegistrationService : IUserRegistrationService
{
    public Task<UserResponse> RegisterAsync(RegisterUserRequest request)
    {
        throw new NotImplementedException();
    }
}