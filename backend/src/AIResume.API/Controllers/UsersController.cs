using AIResume.Application.Users.DTOs;
using AIResume.Application.Users.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AIResume.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserRegistrationService _registrationService;

    public UsersController(
        IUserRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> Register(
        RegisterUserRequest request)
    {
        var response =
            await _registrationService.RegisterAsync(request);

        return StatusCode(StatusCodes.Status201Created, response);
    }
}