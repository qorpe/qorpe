namespace Qorpe.Application.Common.DTOs;

public sealed class PassiveHealthCheckConfigDto
{
    public long? Id { get; set; }

    public bool? Enabled { get; set; }

    public string? Policy { get; set; }

    public TimeSpan? ReactivationPeriod { get; set; }

    // Foreign Key
    public long? HealthCheckConfigId { get; set; }
}
