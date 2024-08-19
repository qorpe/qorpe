namespace Qorpe.Application.Common.DTOs;

public sealed class ClusterConfigDto
{
    public long? Id { get; set; }

    public string ClusterId { get; set; } = default!;

    public string? LoadBalancingPolicy { get; set; }

    public SessionAffinityConfigDto? SessionAffinity { get; set; }

    public HealthCheckConfigDto? HealthCheck { get; set; }

    public HttpClientConfigDto? HttpClient { get; set; }

    public ForwarderRequestConfigDto? HttpRequest { get; set; }

    // public IReadOnlyDictionary<string, DestinationConfig>? Destinations { get; set; }
    public ICollection<DestinationDto>? Destinations { get; set; }

    // public IReadOnlyDictionary<string, string>? Metadata { get; set; }
    public ICollection<ClusterConfigMetadataDto>? Metadata { get; set; }
}
