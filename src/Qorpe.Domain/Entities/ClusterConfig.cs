namespace Qorpe.Domain.Entities;

public sealed class ClusterConfig
{
    public long? Id { get; set; }

    public string? ClusterId { get; set; } = default!;

    public string? LoadBalancingPolicy { get; set; }

    public SessionAffinityConfig? SessionAffinity { get; set; }

    public HealthCheckConfig? HealthCheck { get; set; }

    public HttpClientConfig? HttpClient { get; set; }

    public ForwarderRequestConfig? HttpRequest { get; set; }

    // public IReadOnlyDictionary<string, DestinationConfig>? Destinations { get; set; }
    public ICollection<Destination>? Destinations { get; set; }

    // public IReadOnlyDictionary<string, string>? Metadata { get; set; }
    public ICollection<ClusterConfigMetadata>? Metadata { get; set; }
}
