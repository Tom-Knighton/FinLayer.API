namespace FinLayer.Domain.Users;

public class AppUser
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public required string Auth0Subject { get; set; }
    public bool SignupComplete { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? LastSeenAt { get; set; }
    public bool IsActive { get; set; }
}