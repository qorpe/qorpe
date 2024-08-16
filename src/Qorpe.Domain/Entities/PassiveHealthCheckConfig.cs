namespace Qorpe.Domain.Entities;

public sealed class PassiveHealthCheckConfig
{
    public bool? Enabled { get; init; }

    public string? Policy { get; init; }

    public TimeSpan? ReactivationPeriod { get; init; }
}
