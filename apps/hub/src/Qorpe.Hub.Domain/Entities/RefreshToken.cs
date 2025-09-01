namespace Qorpe.Hub.Domain.Entities;

/** Persisted refresh token (technical/persistence concern). */
public sealed class RefreshToken
{
    public long Id { get; set; }
    public long? TenantId { get; set; }
    public string UserId { get; set; } = null!;
    public string TokenHash { get; set; } = null!;
    public DateTime ExpiresAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? RevokedAtUtc { get; set; }
    public string? DeviceInfo { get; set; }
    public string? CreatedByIp { get; set; }
}