using Qorpe.Domain.Common;

namespace Qorpe.Domain.Entities;

public sealed class RouteConfig : Document
{
    public string? RouteId { get; set; }
    public RouteMatch? Match { get; set; }
    public int? Order { get; set; }
    public string? ClusterId { get; set; }
    public string? AuthorizationPolicy { get; set; }
    public string? RateLimiterPolicy { get; set; }
    public string? OutputCachePolicy { get; set; }
    public string? TimeoutPolicy { get; set; }
    public TimeSpan? Timeout { get; set; }
    public string? CorsPolicy { get; set; }
    public long? MaxRequestBodySize { get; set; }
    public Dictionary<string, string>? Metadata { get; set; }
    public ICollection<Dictionary<string, string>>? Transforms { get; set; }
}
