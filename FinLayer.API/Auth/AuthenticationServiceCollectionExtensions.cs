using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace FinLayer.API.Auth;

public static class AuthenticationServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApiAuthentication(IConfiguration configuration)
        {
            var auth0Options = configuration
                                   .GetRequiredSection(Auth0Options.SectionName)
                                   .Get<Auth0Options>()
                               ?? throw new InvalidOperationException("Auth0 configuration is missing.");
            
            services
                .AddOptions<Auth0Options>()
                .Bind(configuration.GetSection(Auth0Options.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = auth0Options.Authority;
                    options.Audience = auth0Options.Audience;
                    options.MapInboundClaims = false;
                    
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                        RoleClaimType = "permissions"
                    };
                });
            
            return services;
        }
    }
}