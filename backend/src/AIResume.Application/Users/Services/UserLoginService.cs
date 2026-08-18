using AIResume.Application.Users.DTOs;
using AIResume.Application.Users.Interfaces;
using AIResume.Application.Users.Repositories;

namespace AIResume.Application.Users.Services;

public class UserLoginService : IUserLoginService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasherService _passwordHasherService;
    private readonly IJwtTokenService _jwtTokenService;

    public UserLoginService(
        IUserRepository userRepository,
        IPasswordHasherService passwordHasherService,
        IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _passwordHasherService = passwordHasherService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponse> LoginAsync(LoginUserRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user is null)
        {
            throw new InvalidOperationException(
                "Invalid email or password.");
        }

        var passwordIsValid =
            _passwordHasherService.VerifyPassword(
                user.PasswordHash,
                request.Password);

        if (!passwordIsValid)
        {
            throw new InvalidOperationException(
                "Invalid email or password.");
        }

        var token = _jwtTokenService.GenerateToken(user);

        return new LoginResponse
        {
            UserId = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Token = token
        };
    }
}