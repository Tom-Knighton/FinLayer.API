using FinLayer.Domain.Users;

namespace FinLayer.Infrastructure.Persistence.Interfaces;

public interface IUserRepository
{
    Task<AppUser?> GetById(Guid id, CancellationToken ct = default);
    Task<AppUser?> GetByAuth0Subject(string subject, CancellationToken ct = default);
    Task<AppUser> Create(AppUser user, CancellationToken ct = default);
    Task<AppUser> Update(AppUser user, CancellationToken ct = default);
}