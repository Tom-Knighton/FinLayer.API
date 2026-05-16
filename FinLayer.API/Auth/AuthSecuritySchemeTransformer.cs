using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;

namespace FinLayer.API.Auth;

public sealed class AuthSecuritySchemeTransformer(IOptions<Auth0Options> auth0Options): IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var auth0 = auth0Options.Value;
        var authority = auth0.Authority.TrimEnd('/');

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Paste a JWT"
        };

        document.Components.SecuritySchemes["Auth0"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Description = "Login with Auth0",
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new()
                {
                    AuthorizationUrl = new Uri($"{authority}/authorize"),
                    TokenUrl = new Uri($"{authority}/oauth/token"),
                    Scopes = new Dictionary<string, string>
                    {
                        ["openid"] = "OpenID Connect",
                        ["profile"] = "User profile",
                        ["email"] = "User email",
                        [AuthPolicies.ReadMe] = "Read the current FinLayer user",
                        [AuthPolicies.MoneyhubConnect] = "Start Moneyhub connection flow",
                        [AuthPolicies.MoneyhubRead] = "Read Moneyhub data"
                    }
                }
            }
        };
        
        return Task.CompletedTask;
    }
}