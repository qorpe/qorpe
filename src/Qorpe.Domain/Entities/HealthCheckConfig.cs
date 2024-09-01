namespace Qorpe.Domain.Entities;

public sealed class HealthCheckConfig
{
    public PassiveHealthCheckConfig? Passive { get; set; }
    public ActiveHealthCheckConfig? Active { get; set; }
    public string? AvailableDestinationsPolicy { get; set; }
}
