using FinLayer.API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinLayer.API.Controllers;

[ApiController]
[Route("me")]
[Authorize(Policy = AuthPolicies.ReadMe)]
public class MeController: ControllerBase
{
    
    [HttpGet]
    public async Task<IActionResult> GetCurrentUser()
    {
        var subject = User.GetSubject();

        if (string.IsNullOrWhiteSpace(subject))
        {
            return Problem(title: "Authenticated user is missing a subject claim", statusCode: StatusCodes.Status401Unauthorized);
        }
        
        var response = new CurrentUserResponse(
            AuthProvider: "auth0",
            Subject: subject,
            Name: User.Identity?.Name,
            Email: User.GetEmail(),
            Permissions: User.GetPermissions().Order(StringComparer.Ordinal).ToArray());

        return Ok(response);
    }
}

public sealed record CurrentUserResponse(
    string AuthProvider,
    string Subject,
    string? Name,
    string? Email,
    IReadOnlyList<string> Permissions);