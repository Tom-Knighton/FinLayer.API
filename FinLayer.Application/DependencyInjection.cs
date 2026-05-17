using FinLayer.Application.Interfaces;
using FinLayer.Application.Users;
using Microsoft.Extensions.DependencyInjection;

namespace FinLayer.Application;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddFinLayerApplication()
        {
            services.AddTransient<IUserService, UserService>();
            
            return services;
        }
    }
}