namespace Qorpe.Domain.Entities;

public sealed class HealthCheckConfig
{
    public long Id { get; set; }

    public PassiveHealthCheckConfig? Passive { get; set; }

    public ActiveHealthCheckConfig? Active { get; set; }

    public string? AvailableDestinationsPolicy { get; set; }

    // Foreign Key
    public long ClusterConfigId { get; set; }

    // Navigation Property
    // public ClusterConfig? ClusterConfig { get; set; }
}
