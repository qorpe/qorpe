namespace Qorpe.Domain.Entities;

public sealed class PassiveHealthCheckConfig
{
    public long? Id { get; set; }

    public bool? Enabled { get; set; }

    public string? Policy { get; set; }

    public TimeSpan? ReactivationPeriod { get; set; }

    // Foreign Key
    public long? HealthCheckConfigId { get; set; }
}
