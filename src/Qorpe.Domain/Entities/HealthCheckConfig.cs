namespace Qorpe.Domain.Entities;

public sealed class HealthCheckConfig
{
    public PassiveHealthCheckConfig? Passive { get; init; }

    public ActiveHealthCheckConfig? Active { get; init; }

    public string? AvailableDestinationsPolicy { get; init; }
}
