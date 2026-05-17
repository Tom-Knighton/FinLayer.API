using FinLayer.Domain.Users;
using FinLayer.Infrastructure.Persistence.Entities;
using FinLayer.Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinLayer.Infrastructure.Persistence.Repositories;

public class UserRepository(FinLayerDBContext context) : IUserRepository
{
    public async Task<AppUser?> GetById(Guid id, CancellationToken ct = default)
    {
        var entity = await context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Id == id, ct);

        return entity is null ? null : ToDomain(entity);
    }

    public async Task<AppUser?> GetByAuth0Subject(string subject, CancellationToken ct = default)
    {
        var entity = await context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Auth0Subject == subject, ct);

        return entity is null ? null : ToDomain(entity);
    }

    public async Task<AppUser> Create(AppUser user, CancellationToken ct = default)
    {
        var entity = ToEntity(user);

        await context.Users.AddAsync(entity, ct);
        await context.SaveChangesAsync(ct);

        return ToDomain(entity);
    }

    public async Task<AppUser> Update(AppUser user, CancellationToken ct = default)
    {
        var entity = await context.Users
            .SingleOrDefaultAsync(
                existingUser => existingUser.Id == user.Id,
                ct);

        if (entity is null)
        {
            throw new InvalidOperationException(
                $"User '{user.Id}' could not be found.");
        }

        entity.Auth0Subject = user.Auth0Subject;
        entity.SignupComplete = user.SignupComplete;
        entity.CreatedAt = user.CreatedAt;
        entity.UpdatedAt = user.UpdatedAt;
        entity.LastSeenAt = user.LastSeenAt;
        entity.Name = user.Name;
        entity.IsActive = user.IsActive;

        await context.SaveChangesAsync(ct);

        return ToDomain(entity);
    }

    private static AppUser ToDomain(UserEntity entity)
    {
        return new AppUser
        {
            Id = entity.Id,
            Auth0Subject = entity.Auth0Subject,
            SignupComplete = entity.SignupComplete,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            LastSeenAt = entity.LastSeenAt,
            IsActive = entity.IsActive,
            Name = entity.Name
        };
    }

    private static UserEntity ToEntity(AppUser user)
    {
        return new UserEntity
        {
            Id = user.Id,
            Auth0Subject = user.Auth0Subject,
            SignupComplete = user.SignupComplete,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            LastSeenAt = user.LastSeenAt,
            IsActive = user.IsActive,
            Name = user.Name
        };
    }
}