namespace Qorpe.Application.Common.DTOs;

public sealed class HealthCheckConfigDto
{
    public long? Id { get; set; }

    public PassiveHealthCheckConfigDto? Passive { get; set; }

    public ActiveHealthCheckConfigDto? Active { get; set; }

    public string? AvailableDestinationsPolicy { get; set; }

    // Foreign Key
    public long? ClusterConfigId { get; set; }

    // Navigation Property
    // public ClusterConfig? ClusterConfig { get; set; }
}
