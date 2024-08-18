namespace Qorpe.Application.Common.DTOs;

public sealed class SessionAffinityConfigDto
{
    public long? Id { get; set; }

    public bool? Enabled { get; set; }

    public string? Policy { get; set; }

    public string? FailurePolicy { get; set; }

    public string AffinityKeyName { get; set; } = default!;

    public SessionAffinityCookieConfigDto? Cookie { get; set; }

    // Foreign Key
    public long? ClusterConfigId { get; set; }

    // Navigation Property
    // public ClusterConfig? ClusterConfig { get; set; }
}
