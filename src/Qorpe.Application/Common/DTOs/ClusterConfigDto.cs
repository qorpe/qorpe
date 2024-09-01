using Qorpe.Domain.Common;

namespace Qorpe.Application.Common.DTOs;

public sealed class ClusterConfigDto : Document
{
    public string ClusterId { get; set; } = default!;
    public string? LoadBalancingPolicy { get; set; }
    public SessionAffinityConfigDto? SessionAffinity { get; set; }
    public HealthCheckConfigDto? HealthCheck { get; set; }
    public HttpClientConfigDto? HttpClient { get; set; }
    public ForwarderRequestConfigDto? HttpRequest { get; set; }
    public Dictionary<string, DestinationConfigDto>? Destinations { get; set; }
    public IReadOnlyDictionary<string, string>? Metadata { get; set; }
}
