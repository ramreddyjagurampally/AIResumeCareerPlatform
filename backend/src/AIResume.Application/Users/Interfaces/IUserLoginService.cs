using AIResume.Application.Users.DTOs;

namespace AIResume.Application.Users.Interfaces;

public interface IUserLoginService
{
    Task<LoginResponse> LoginAsync(LoginUserRequest request);
}