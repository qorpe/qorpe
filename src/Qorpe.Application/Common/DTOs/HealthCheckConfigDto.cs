namespace Qorpe.Application.Common.DTOs;

public sealed class HealthCheckConfigDto
{
    public PassiveHealthCheckConfigDto? Passive { get; set; }
    public ActiveHealthCheckConfigDto? Active { get; set; }
    public string? AvailableDestinationsPolicy { get; set; }
}
