namespace Qorpe.Domain.Entities;

public sealed class HealthCheckConfig
{
    public long Id { get; set; }
    public PassiveHealthCheckConfig? Passive { get; init; }

    public ActiveHealthCheckConfig? Active { get; init; }

    public string? AvailableDestinationsPolicy { get; init; }

    // Foreign Key
    public long ClusterConfigId { get; set; }

    // Navigation Property
    // public ClusterConfig? ClusterConfig { get; set; }
}
