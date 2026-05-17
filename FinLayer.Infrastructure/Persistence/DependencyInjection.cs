using FinLayer.Infrastructure.Persistence;
using FinLayer.Infrastructure.Persistence.Interfaces;
using FinLayer.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinLayer.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddPersistence(IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString("FinLayerDb");

            if (string.IsNullOrWhiteSpace(connString))
            {
                throw new InvalidOperationException("Missing connection string for FinLayerDb.");
            }

            services.AddDbContext<FinLayerDBContext>(options =>
            {
                options.UseNpgsql(connString,
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsHistoryTable(
                            "__EFMigrationsHistory",
                            "finlayer");
                    });
            });

            services.AddScoped<IUserRepository, UserRepository>();
            
            return services;
        }
    }
}