using Microsoft.AspNetCore.Identity;

namespace Qorpe.Hub.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public long? TenantId { get; set; }
    public string? DisplayName { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<RefreshToken>? RefreshTokens { get; set; }
        = new List<RefreshToken>();
}