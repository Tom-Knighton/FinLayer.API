using FinLayer.Domain.Users;

namespace FinLayer.Application.Interfaces;

public interface IUserService
{
    Task<AppUser> GetUserByAuth0Subject(string? subject, CancellationToken ct = default);
    Task<AppUser> CreateUser(string? subject, SignupRequest request, CancellationToken ct = default);
}