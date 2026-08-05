using AIResume.Application.Users.DTOs;

namespace AIResume.Application.Users.Interfaces;

public interface IUserRegistrationService
{
    Task<UserResponse> RegisterAsync(RegisterUserRequest request);
}
  
