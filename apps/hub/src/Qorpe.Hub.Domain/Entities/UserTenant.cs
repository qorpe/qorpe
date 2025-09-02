namespace Qorpe.Hub.Domain.Entities;

public class UserTenant
{
    public string UserId { get; init; } = null!;
    public long TenantId { get; init; }
    public string Role { get; init; } = null!;
    public bool IsActive { get; init; } = true;
    public DateTime CreatedAtUtc { get; init; } = DateTime.UtcNow;

    public ApplicationUser User { get; init; } = null!;
    public Tenant Tenant { get; init; } = null!;
}