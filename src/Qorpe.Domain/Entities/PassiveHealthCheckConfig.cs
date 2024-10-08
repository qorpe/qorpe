﻿namespace Qorpe.Domain.Entities;

public sealed class PassiveHealthCheckConfig
{
    public bool? Enabled { get; set; }
    public string? Policy { get; set; }
    public TimeSpan? ReactivationPeriod { get; set; }
}
