namespace Qorpe.Domain.Entities;

public sealed class RouteConfig
{
    public long? Id { get; set; }

    public string RouteId { get; set; } = default!;

    public RouteMatch? Match { get; set; } = default!;

    public int? Order { get; set; }

    public string? ClusterId { get; set; }

    public string? AuthorizationPolicy { get; set; }

    public string? RateLimiterPolicy { get; set; }

    public string? OutputCachePolicy { get; set; }

    public string? TimeoutPolicy { get; set; }

    public TimeSpan? Timeout { get; set; }

    public string? CorsPolicy { get; set; }

    public long? MaxRequestBodySize { get; set; }

    // public IReadOnlyDictionary<string, string>? Metadata { get; set; }
    public ICollection<Metadata>? Metadata { get; set; }

    // public IReadOnlyList<IReadOnlyDictionary<string, string>>? Transforms { get; set; }
    public ICollection<Transform>? Transforms { get; set; }
}
