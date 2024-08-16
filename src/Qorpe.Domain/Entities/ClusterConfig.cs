namespace Qorpe.Domain.Entities;

public sealed class ClusterConfig
{
    public string ClusterId { get; init; } = default!;

    public string? LoadBalancingPolicy { get; init; }

    public SessionAffinityConfig? SessionAffinity { get; init; }

    public HealthCheckConfig? HealthCheck { get; init; }

    public HttpClientConfig? HttpClient { get; init; }

    public ForwarderRequestConfig? HttpRequest { get; init; }

    public IReadOnlyDictionary<string, DestinationConfig>? Destinations { get; init; }

    public IReadOnlyDictionary<string, string>? Metadata { get; init; }
}
