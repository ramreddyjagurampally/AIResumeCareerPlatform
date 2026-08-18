using System.Security.Claims;
using AIResume.Application.Users.DTOs;
using AIResume.Application.Users.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIResume.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserRegistrationService _registrationService;
    private readonly IUserLoginService _loginService;

    public UsersController(
        IUserRegistrationService registrationService,
        IUserLoginService loginService)
    {
        _registrationService = registrationService;
        _loginService = loginService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> Register(
        RegisterUserRequest request)
    {
        var response =
            await _registrationService.RegisterAsync(request);

        return StatusCode(
            StatusCodes.Status201Created,
            response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(
        LoginUserRequest request)
    {
        var response =
            await _loginService.LoginAsync(request);

        return Ok(response);
    }

    [Authorize]
    [HttpGet("profile")]
    public IActionResult GetProfile()
    {
        var userId =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("sub")?.Value;

        var email =
            User.FindFirst(ClaimTypes.Email)?.Value
            ?? User.FindFirst("email")?.Value;

        var firstName =
            User.FindFirst("firstName")?.Value;

        var lastName =
            User.FindFirst("lastName")?.Value;

        return Ok(new
        {
            UserId = userId,
            Email = email,
            FirstName = firstName,
            LastName = lastName
        });
    }
}