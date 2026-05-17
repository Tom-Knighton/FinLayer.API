using FinLayer.Application.Interfaces;
using FinLayer.Domain.Exceptions;
using FinLayer.Domain.Users;
using FinLayer.Infrastructure.Persistence.Interfaces;

namespace FinLayer.Application.Users;

public class UserService(IUserRepository repository): IUserService
{
    public async Task<AppUser> GetUserByAuth0Subject(string? subject, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(subject))
        {
            throw new ArgumentException(
                "Subject cannot be null or empty.",
                nameof(subject));
        }

        var user = await repository.GetByAuth0Subject(subject, ct);

        if (user is null)
        {
            throw new UserNotFoundException();
        }

        return user;
    }

    public async Task<AppUser> CreateUser(string? subject, SignupRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(subject))
        {
            throw new ArgumentException(
                "Subject cannot be null or empty.",
                nameof(subject));
        }

        var existingUser = await repository.GetByAuth0Subject(subject, ct);

        if (existingUser is not null)
        {
            throw new UserSignupConflictException();
        }

        var newUser = new AppUser
        {
            Id = Guid.NewGuid(),
            Auth0Subject = subject,
            Name = request.Name,
            SignupComplete = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            LastSeenAt = DateTime.UtcNow,
            IsActive = true
        };

        return await repository.Create(newUser, ct);
    }
}