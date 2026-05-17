using FinLayer.API.Auth;
using FinLayer.Application.Interfaces;
using FinLayer.Domain.Exceptions;
using FinLayer.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinLayer.API.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicies.ReadMe)]
public class CurrentUserController(IUserService userService, ILogger<CurrentUserController> log): ControllerBase
{
    [HttpGet("/me")]
    [ProducesResponseType<CurrentUser>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentUser(CancellationToken ct = default)
    {
        try
        {
            var subject = User.GetSubject();
            var user = await userService.GetUserByAuth0Subject(subject, ct);

            var response = new CurrentUser
            {
                Id = user.Id,
                Name = user.Name
            };

            return Ok(response);
        }
        catch (UserNotFoundException ex)
        {
            log.LogError(ex, "Current user not found for subject '{Subject}'", User.GetSubject());
            return NotFound();
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Failed to get current user for subject: ${Subject}", User.GetSubject());
            return Problem("An unexpected error occurred.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost("/signup")]
    public async Task<IActionResult> Signup([FromBody] SignupRequest request, CancellationToken ct = default)
    {
        try
        {
            var subject = User.GetSubject();
            var user = await userService.CreateUser(subject, request, ct);

            var response = new CurrentUser
            {
                Id = user.Id,
                Name = user.Name
            };

            return Ok(response);
        }
        catch (UserSignupConflictException ex)
        {
            log.LogError(ex, "User signup conflict for subject '{Subject}'", User.GetSubject());
            return Conflict("A user with the same subject already exists.");
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Failed to signup user for subject: ${Subject}", User.GetSubject());
            return Problem("An unexpected error occurred.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }
    
}