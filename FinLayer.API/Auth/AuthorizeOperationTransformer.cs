using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace FinLayer.Api.Auth;

public sealed class AuthorizeOperationTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(
        OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken)
    {
        var endpointMetadata = context.Description.ActionDescriptor.EndpointMetadata;

        var allowsAnonymous = endpointMetadata
            .OfType<AllowAnonymousAttribute>()
            .Any();

        if (allowsAnonymous)
        {
            return Task.CompletedTask;
        }

        var authorizeData = endpointMetadata
            .OfType<IAuthorizeData>()
            .ToArray();

        if (authorizeData.Length == 0)
        {
            return Task.CompletedTask;
        }

        var requiredScopes = authorizeData
            .Select(data => data.Policy)
            .Where(policy => !string.IsNullOrWhiteSpace(policy))
            .Distinct(StringComparer.Ordinal)
            .ToList();

        operation.Security ??= [];

        operation.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference(
                "Bearer",
                context.Document!)] = []
        });

        operation.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Auth0",
                context.Document!)] = requiredScopes
        });

        return Task.CompletedTask;
    }
}