using Qorpe.Domain.Common;

namespace Qorpe.Domain.Entities.Lite;

public sealed class ClusterConfig : DocumentLite
{
    public string? ClusterId { get; set; } = default!;
    public string? LoadBalancingPolicy { get; set; }
    public SessionAffinityConfig? SessionAffinity { get; set; }
    public HealthCheckConfig? HealthCheck { get; set; }
    public HttpClientConfig? HttpClient { get; set; }
    public ForwarderRequestConfig? HttpRequest { get; set; }
    public Dictionary<string, DestinationConfig>? Destinations { get; set; }
    public Dictionary<string, string>? Metadata { get; set; }
}
