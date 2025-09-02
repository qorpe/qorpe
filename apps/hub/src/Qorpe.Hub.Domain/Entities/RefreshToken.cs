namespace Qorpe.Hub.Domain.Entities;

/** Persisted refresh token (technical/persistence concern). */
public sealed class RefreshToken
{
    public long Id { get; init; }
    public long TenantId { get; init; }
    public string UserId { get; init; } = null!;
    public string TokenHash { get; init; } = null!;
    public DateTime ExpiresAtUtc { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime? RevokedAtUtc { get; set; }
    public string? DeviceInfo { get; init; }
    public string? CreatedByIp { get; init; }
    
    public ApplicationUser? User { get; init; }
    public Tenant? Tenant { get; init; }
}