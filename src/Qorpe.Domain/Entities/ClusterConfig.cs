namespace Qorpe.Domain.Entities;

public sealed class ClusterConfig
{
    public long Id { get; set; }

    public required string ClusterId { get; init; } = default!;

    public string? LoadBalancingPolicy { get; init; }

    public SessionAffinityConfig? SessionAffinity { get; init; }

    public HealthCheckConfig? HealthCheck { get; init; }

    public HttpClientConfig? HttpClient { get; init; }

    public ForwarderRequestConfig? HttpRequest { get; init; }

    // public IReadOnlyDictionary<string, DestinationConfig>? Destinations { get; init; }
    public ICollection<Destination>? Destinations { get; set; }

    // public IReadOnlyDictionary<string, string>? Metadata { get; init; }
    public ICollection<Metadata>? Metadata { get; init; }
}
