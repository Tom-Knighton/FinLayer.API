using FinLayer.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinLayer.Infrastructure.Persistence.Configurations;

public sealed class UserEntityConfiguration: IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users");
        
        builder.HasKey(u => u.Id);
        builder
            .HasIndex(u => u.Auth0Subject)
            .IsUnique();
        builder
            .Property(u => u.SignupComplete)
            .HasDefaultValue(false)
            .IsRequired();
    }
}