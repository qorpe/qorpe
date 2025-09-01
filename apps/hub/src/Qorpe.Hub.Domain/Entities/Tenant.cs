namespace Qorpe.Hub.Domain.Entities;

/** Tenant aggregate persisted in a Hub authority database. */
public class Tenant
{
    public long Id { get; set; }
    public string Key { get; set; } = null!;  // URL-safe slug (e.g., "acme")
    public string Name { get; set; } = null!; // Display name
    public string? Domain { get; set; }          // Optional custom domain (e.g., acme.com)
    public bool IsActive { get; set; } = true;

    // Optional details
    public string? Timezone { get; set; }        // e.g., "Europe/Istanbul"
    public Dictionary<string, string>? Metadata { get; set; } // Stored as jsonb

    // Audit
    public DateTime CreatedAtUtc { get; set; }   // default now() at time zone 'utc'
    public DateTime? UpdatedAtUtc { get; set; }  // updated by app logic

    // Concurrency (PostgreSQL min is recommended; see config)
    // Navigation
    public ICollection<ApplicationUser>? Users { get; set; }
        = new List<ApplicationUser>();
    
    public ICollection<RefreshToken>? RefreshTokens { get; set; }
        = new List<RefreshToken>();
}