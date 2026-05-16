using System.Security.Claims;

namespace FinLayer.API.Auth;

public static class ClaimsPrincipalExtensions
{
    extension(ClaimsPrincipal user)
    {
        public string? GetSubject()
        {
            return user.FindFirstValue("sub");
        }
        
        public string? GetEmail()
        {
            return user.FindFirstValue("email");
        }

        public bool HasPermission(string permission)
        {
            return user.GetPermissions().Contains(permission);
        }
        
        public IReadOnlySet<string> GetPermissions() 
        {
            var permissions = user
                .FindAll("permissions")
                .Select(claim => claim.Value);

            var scopes = user
                             .FindFirstValue("scope")?
                             .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                         ?? [];

            return permissions
                .Concat(scopes)
                .ToHashSet(StringComparer.Ordinal);
        }
    }
}