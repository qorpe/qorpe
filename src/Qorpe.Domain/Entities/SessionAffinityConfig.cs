namespace Qorpe.Domain.Entities;

public sealed class SessionAffinityConfig
{
    public long Id { get; set; }

    public bool? Enabled { get; set; }

    public string? Policy { get; set; }

    public string? FailurePolicy { get; set; }

    public string AffinityKeyName { get; set; } = default!;

    public SessionAffinityCookieConfig? Cookie { get; set; }

    // Foreign Key
    public long ClusterConfigId { get; set; }

    // Navigation Property
    // public ClusterConfig? ClusterConfig { get; set; }
}
