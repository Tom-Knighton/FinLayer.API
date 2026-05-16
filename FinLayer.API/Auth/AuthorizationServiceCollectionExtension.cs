using System.Security.Claims;

namespace FinLayer.API.Auth;

public static class AuthorizationServiceCollectionExtension
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApiAuthorization()
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthPolicies.ReadMe, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireAssertion(context =>
                        context.User.HasPermission(AuthPolicies.ReadMe));
                });

                options.AddPolicy(AuthPolicies.MoneyhubConnect, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireAssertion(context =>
                        context.User.HasPermission(AuthPolicies.MoneyhubConnect));
                });

                options.AddPolicy(AuthPolicies.MoneyhubRead, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireAssertion(context =>
                        context.User.HasPermission(AuthPolicies.MoneyhubRead));
                });
            });

            return services;
        }
    }
}