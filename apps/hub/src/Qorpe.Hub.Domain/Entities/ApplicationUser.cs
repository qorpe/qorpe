using Microsoft.AspNetCore.Identity;

namespace Qorpe.Hub.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; init; }
    public bool IsActive { get; init; } = true;
    public ICollection<RefreshToken>? RefreshTokens { get; init; }
        = new List<RefreshToken>();
    public ICollection<UserTenant> Memberships { get; init; } 
        = new List<UserTenant>();
}