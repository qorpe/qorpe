using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Qorpe.Application.Common.Interfaces;
using Qorpe.Domain.Entities;

namespace Qorpe.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<ActiveHealthCheckConfig> ActiveHealthCheckConfigs { get; set; }
    public DbSet<ClusterConfig> ClusterConfigs { get; set; }
    public DbSet<Destination> Destinations { get; set; }
    public DbSet<DestinationConfig> DestinationConfigs { get; set; }
    public DbSet<ForwarderRequestConfig> ForwarderRequestConfigs { get; set; }
    public DbSet<HealthCheckConfig> HealthCheckConfigs { get; set; }
    public DbSet<HttpClientConfig> HttpClientConfigs { get; set; }
    public DbSet<Metadata> Metadata { get; set; }
    public DbSet<PassiveHealthCheckConfig> PassiveHealthCheckConfigs { get; set; }
    public DbSet<RouteConfig> RouteConfigs { get; set; }
    public DbSet<RouteHeader> RouteHeaders { get; set; }
    public DbSet<RouteMatch> RouteMatches { get; set; }
    public DbSet<RouteQueryParameter> RouteQueryParameters { get; set; }
    public DbSet<SessionAffinityConfig> SessionAffinityConfigs { get; set; }
    public DbSet<SessionAffinityCookieConfig> SessionAffinityCookieConfigs { get; set; }
    public DbSet<Transform> Transforms { get; set; }
    public DbSet<WebProxyConfig> WebProxyConfigs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var timeSpanStringConverter = new ValueConverter<TimeSpan, string>(
        v => v.ToString(), // TimeSpan -> String (örn: '01:30:00')
        v => TimeSpan.Parse(v)); // String -> TimeSpan

        modelBuilder.Entity<ActiveHealthCheckConfig>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                  .ValueGeneratedOnAdd();

            entity.Property(x => x.Interval)
                  .HasConversion(timeSpanStringConverter);

            entity.Property(x => x.Timeout)
                  .HasConversion(timeSpanStringConverter);
        });

        modelBuilder.Entity<ClusterConfig>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                  .ValueGeneratedOnAdd();

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

        modelBuilder.Entity<Destination>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                  .ValueGeneratedOnAdd();

            entity.HasOne(x => x.Value)
                  .WithOne()
                  .HasForeignKey<DestinationConfig>(x => x.DestinationId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DestinationConfig>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                  .ValueGeneratedOnAdd();

            entity.HasMany(x => x.Metadata)
                  .WithOne()
                  .HasForeignKey(x => x.ParentId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ForwarderRequestConfig>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                  .ValueGeneratedOnAdd();

            entity.Property(x => x.Version)
                  .HasConversion(
                      x => x != null ? x.ToString() : null, // Version to string
                      x => string.IsNullOrEmpty(x) ? new Version() : Version.Parse(x) // string to Version
                  );

            entity.Property(x => x.ActivityTimeout)
                  .HasConversion(timeSpanStringConverter);
        });

        modelBuilder.Entity<HealthCheckConfig>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                  .ValueGeneratedOnAdd();

            entity.HasOne(x => x.Passive)
                  .WithOne()
                  .HasForeignKey<PassiveHealthCheckConfig>(x => x.HealthCheckConfigId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Active)
                  .WithOne()
                  .HasForeignKey<ActiveHealthCheckConfig>(x => x.HealthCheckConfigId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<HttpClientConfig>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                  .ValueGeneratedOnAdd();

            entity.HasOne(x => x.WebProxy)
                  .WithOne()
                  .HasForeignKey<WebProxyConfig>(x => x.HttpClientConfigId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Metadata>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                  .ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<PassiveHealthCheckConfig>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                  .ValueGeneratedOnAdd();

            entity.Property(x => x.ReactivationPeriod)
                  .HasConversion(timeSpanStringConverter);
        });

        modelBuilder.Entity<RouteConfig>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                  .ValueGeneratedOnAdd();

            entity.HasOne(x => x.Match)
                  .WithOne()
                  .HasForeignKey<RouteMatch>(x => x.RouteConfigId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(x => x.Metadata)
                  .WithOne()    
                  .HasForeignKey(x => x.ParentId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(x => x.Transforms)
                  .WithOne()
                  .HasForeignKey(x => x.RouteConfigId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.Property(x => x.Timeout)
                  .HasConversion(timeSpanStringConverter);
        });

        modelBuilder.Entity<RouteHeader>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                  .ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<RouteMatch>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                  .ValueGeneratedOnAdd();

            entity.HasMany(x => x.QueryParameters)
                  .WithOne()
                  .HasForeignKey(x => x.RouteMatchId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<RouteQueryParameter>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                  .ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<SessionAffinityConfig>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                  .ValueGeneratedOnAdd();

            entity.HasOne(x => x.Cookie)
                  .WithOne()
                  .HasForeignKey<SessionAffinityCookieConfig>(x => x.SessionAffinityConfigId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SessionAffinityCookieConfig>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                  .ValueGeneratedOnAdd();

            entity.Property(x => x.Expiration)
                  .HasConversion(timeSpanStringConverter);

            entity.Property(x => x.MaxAge)
                  .HasConversion(timeSpanStringConverter);
        });

        modelBuilder.Entity<Transform>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                  .ValueGeneratedOnAdd();

            entity.HasMany(x => x.Metadata)
                  .WithOne()
                  .HasForeignKey(x => x.ParentId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<WebProxyConfig>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                  .ValueGeneratedOnAdd();

            entity.Property(x => x.Address)
                  .HasConversion(
                      x => x != null ? x.ToString() : null, // Uri to string
                      x => x != null ? new Uri(x) : null // string to Uri
                  );
        });
    }
}
