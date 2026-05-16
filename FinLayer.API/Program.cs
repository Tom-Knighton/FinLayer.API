using System.Reflection;
using FinLayer.Api.Auth;
using FinLayer.API.Auth;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi(o =>
{
    o.AddDocumentTransformer<AuthSecuritySchemeTransformer>();
    o.AddOperationTransformer<AuthorizeOperationTransformer>();
});

var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{environmentName}.json", false)
    .AddEnvironmentVariables();

if (environmentName == "Development")
{
    builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());
}

builder.Services
    .AddApiAuthentication(builder.Configuration)
    .AddApiAuthorization()
    .AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/docs", options =>
    {
        options
            .WithTitle("FinLayer API")
            .AddPreferredSecuritySchemes("Bearer", "Auth0")
            .AddAuthorizationCodeFlow("Auth0", flow =>
            {
                flow.ClientId = app.Configuration["Auth0:ClientId"];
                flow.Pkce = Pkce.Sha256;
                flow.SelectedScopes =
                [
                    "openid",
                    "profile",
                    "email",
                    AuthPolicies.ReadMe,
                    AuthPolicies.MoneyhubConnect,
                    AuthPolicies.MoneyhubRead
                ];

                flow.AddQueryParameter(
                    "audience",
                    app.Configuration["Auth0:Audience"]!);
            });
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapControllers();

app.Run();