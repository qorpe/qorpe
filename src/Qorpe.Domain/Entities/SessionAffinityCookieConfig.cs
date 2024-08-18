using Qorpe.Domain.Enums;

namespace Qorpe.Domain.Entities;

public sealed class SessionAffinityCookieConfig
{
    public long Id { get; set; }

    public string? Path { get; set; }

    public string? Domain { get; set; }

    public bool? HttpOnly { get; set; }

    public CookieSecurePolicy? SecurePolicy { get; set; }

    public SameSiteMode? SameSite { get; set; }

    public TimeSpan? Expiration { get; set; }

    public TimeSpan? MaxAge { get; set; }

    public bool? IsEssential { get; set; }

    // Foreign Key
    public long SessionAffinityConfigId { get; set; }
}
