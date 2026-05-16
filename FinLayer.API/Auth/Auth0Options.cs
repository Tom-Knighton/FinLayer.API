using System.ComponentModel.DataAnnotations;

namespace FinLayer.API.Auth;

public sealed class Auth0Options
{
    public const string SectionName = "Auth0";

    [Required]
    public required string Authority { get; init; }

    [Required]
    public required string Audience { get; init; }
}