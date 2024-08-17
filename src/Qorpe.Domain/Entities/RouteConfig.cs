namespace Qorpe.Domain.Entities;

public sealed class RouteConfig
{
    public long Id { get; set; }

    public string RouteId { get; init; } = default!;

    public RouteMatch Match { get; init; } = default!;

    public int? Order { get; init; }

    public string? ClusterId { get; init; }

    public string? AuthorizationPolicy { get; init; }

    public string? RateLimiterPolicy { get; init; }

    public string? OutputCachePolicy { get; init; }

    public string? TimeoutPolicy { get; init; }

    public TimeSpan? Timeout { get; init; }

    public string? CorsPolicy { get; init; }

    public long? MaxRequestBodySize { get; init; }

    public IReadOnlyDictionary<string, string>? Metadata { get; init; }

    public IReadOnlyList<IReadOnlyDictionary<string, string>>? Transforms { get; init; }
}
