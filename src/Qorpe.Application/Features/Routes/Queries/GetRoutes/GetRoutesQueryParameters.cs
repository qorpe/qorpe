using Qorpe.Application.Common.Models;

namespace Qorpe.Application.Features.Routes.Queries.GetRoutes;

public class GetRoutesQueryParameters : PaginationOptions
{
    public long? Id { get; set; }

    public string? RouteId { get; set; } = default!;

    public int? Order { get; set; }

    public string? ClusterId { get; set; }

    public string? AuthorizationPolicy { get; set; }

    public string? RateLimiterPolicy { get; set; }

    public string? OutputCachePolicy { get; set; }

    public string? TimeoutPolicy { get; set; }

    public TimeSpan? Timeout { get; set; }

    public string? CorsPolicy { get; set; }

    public long? MaxRequestBodySize { get; set; }

    public bool? FromCache { get; set; }
}
