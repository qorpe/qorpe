using Microsoft.EntityFrameworkCore;
using Qorpe.Domain.Entities;

namespace Qorpe.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<ActiveHealthCheckConfig> ActiveHealthCheckConfigs { get; set; }
    public DbSet<ClusterConfig> ClusterConfigs { get; set; }
    public DbSet<DestinationConfig> DestinationConfigs { get; set; }
    public DbSet<ForwarderRequestConfig> ForwarderRequestConfigs { get; set; }
    public DbSet<HealthCheckConfig> HealthCheckConfigs { get; set; }
    public DbSet<HttpClientConfig> HttpClientConfigs { get; set; }
    public DbSet<PassiveHealthCheckConfig> PassiveHealthCheckConfigs { get; set; }
    public DbSet<RouteConfig> RouteConfigs { get; set; }
    public DbSet<RouteHeader> RouteHeaders { get; set; }
    public DbSet<RouteMatch> RouteMatches { get; set; }
    public DbSet<RouteQueryParameter> RouteQueryParameters { get; set; }
    public DbSet<SessionAffinityConfig> SessionAffinityConfigs { get; set; }
    public DbSet<SessionAffinityCookieConfig> SessionAffinityCookieConfigs { get; set; }
    public DbSet<WebProxyConfig> WebProxyConfigs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ActiveHealthCheckConfig>(entity =>
        {
            entity.HasKey(x => x.Id);
        });

        modelBuilder.Entity<ClusterConfig>(entity =>
        {
            entity.HasKey(x => x.ClusterId);

            entity.HasOne(x => x.SessionAffinity)
                  .WithOne()
                  .HasForeignKey<SessionAffinityConfig>(x => x.ClusterConfigId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.HealthCheck)
                  .WithOne()
                  .HasForeignKey<HealthCheckConfig>(x => x.ClusterConfigId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.HttpClient)
                  .WithOne()
                  .HasForeignKey<HttpClientConfig>(x => x.ClusterConfigId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.HttpRequest)
                  .WithOne()
                  .HasForeignKey<ForwarderRequestConfig>(x => x.ClusterConfigId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(x => x.Destinations)
                  .WithOne()
                  .HasForeignKey(x => x.ParentId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(x => x.Metadata)
                  .WithOne()
                  .HasForeignKey(x => x.ParentId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
