using Qorpe.Domain.Common;

namespace Qorpe.Domain.Entities;

public sealed class ClusterConfig : Document
{
    public string? ClusterId { get; set; }
    public string? LoadBalancingPolicy { get; set; }
    public SessionAffinityConfig? SessionAffinity { get; set; }
    public HealthCheckConfig? HealthCheck { get; set; }
    public HttpClientConfig? HttpClient { get; set; }
    public ForwarderRequestConfig? HttpRequest { get; set; }
    public Dictionary<string, DestinationConfig>? Destinations { get; set; }
    public Dictionary<string, string>? Metadata { get; set; }
}
