namespace Qorpe.Domain.Entities;

public sealed class SessionAffinityConfig
{
    public long Id { get; set; }

    public bool? Enabled { get; init; }

    public string? Policy { get; init; }

    public string? FailurePolicy { get; init; }

    public string AffinityKeyName { get; init; } = default!;

    public SessionAffinityCookieConfig? Cookie { get; init; }

    // Foreign Key
    public long ClusterConfigId { get; set; }

    // Navigation Property
    // public ClusterConfig? ClusterConfig { get; set; }
}
