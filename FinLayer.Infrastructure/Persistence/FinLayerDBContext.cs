using FinLayer.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinLayer.Infrastructure.Persistence;

public sealed class FinLayerDBContext(DbContextOptions<FinLayerDBContext> options): DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("finlayer");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinLayerDBContext).Assembly);
    }
}