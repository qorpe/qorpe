namespace Qorpe.Application.Common.DTOs;

public sealed class PassiveHealthCheckConfigDto
{
    public bool? Enabled { get; set; }
    public string? Policy { get; set; }
    public TimeSpan? ReactivationPeriod { get; set; }
}
