namespace Qorpe.Hub.Domain.Entities;

/** Tenant aggregate persisted in a Hub authority database. */
public class Tenant
{
    public long Id { get; init; }
    public string Key { get; init; } = null!;  // URL-safe slug (e.g., "acme")
    public string Name { get; init; } = null!; // Display name
    public string? Domain { get; init; }          // Optional custom domain (e.g., acme.com)
    public bool IsActive { get; init; } = true;

    // Optional details
    public string? Timezone { get; init; }        // e.g., "Europe/Istanbul"
    public Dictionary<string, string>? Metadata { get; init; } // Stored as jsonb

    // Audit
    public DateTime CreatedAtUtc { get; init; }   // default now() at time zone 'utc'
    public DateTime? UpdatedAtUtc { get; init; }  // updated by app logic

    // Concurrency (PostgreSQL min is recommended; see config)
    // Navigation
    public ICollection<UserTenant> Memberships { get; init; } 
        = new List<UserTenant>();
    
    public ICollection<RefreshToken>? RefreshTokens { get; init; }
        = new List<RefreshToken>();
}