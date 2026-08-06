using AIResume.Application.Users.DTOs;
using AIResume.Application.Users.Interfaces;
using AIResume.Application.Users.Repositories;
using AIResume.Domain.Entities;
using FluentValidation;

namespace AIResume.Application.Users.Services;

public class UserRegistrationService : IUserRegistrationService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<RegisterUserRequest> _validator;
    private readonly IPasswordHasherService _passwordHasherService;

    public UserRegistrationService(
        IUserRepository userRepository,
        IValidator<RegisterUserRequest> validator,
        IPasswordHasherService passwordHasherService)
    {
        _userRepository = userRepository;
        _validator = validator;
        _passwordHasherService = passwordHasherService;
    }

    public async Task<UserResponse> RegisterAsync(
        RegisterUserRequest request)
    {
        var validationResult =
            await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(
                validationResult.Errors);
        }

        var emailExists =
            await _userRepository.EmailExistsAsync(request.Email);

        if (emailExists)
        {
            throw new InvalidOperationException(
                "A user with this email already exists.");
        }

        var passwordHash =
            _passwordHasherService.HashPassword(request.Password);

        var user = new User(
            request.FirstName,
            request.LastName,
            request.Email,
            passwordHash);

        await _userRepository.AddAsync(user);

        return new UserResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            CreatedAtUtc = user.CreatedAtUtc
        };
    }
}