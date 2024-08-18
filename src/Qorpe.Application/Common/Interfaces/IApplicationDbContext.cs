using Microsoft.EntityFrameworkCore;
using Qorpe.Domain.Entities;

namespace Qorpe.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<ActiveHealthCheckConfig> ActiveHealthCheckConfigs { get; }
    public DbSet<ClusterConfig> ClusterConfigs { get; }
    public DbSet<Destination> Destinations { get; }
    public DbSet<DestinationConfig> DestinationConfigs { get; }
    public DbSet<ForwarderRequestConfig> ForwarderRequestConfigs { get; }
    public DbSet<HealthCheckConfig> HealthCheckConfigs { get; }
    public DbSet<HttpClientConfig> HttpClientConfigs { get; }
    public DbSet<Metadata> Metadata { get; }
    public DbSet<PassiveHealthCheckConfig> PassiveHealthCheckConfigs { get; }
    public DbSet<RouteConfig> RouteConfigs { get; }
    public DbSet<RouteHeader> RouteHeaders { get; }
    public DbSet<RouteMatch> RouteMatches { get; }
    public DbSet<RouteQueryParameter> RouteQueryParameters { get; }
    public DbSet<SessionAffinityConfig> SessionAffinityConfigs { get; }
    public DbSet<SessionAffinityCookieConfig> SessionAffinityCookieConfigs { get; }
    public DbSet<Transform> Transforms { get; }
    public DbSet<WebProxyConfig> WebProxyConfigs { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
