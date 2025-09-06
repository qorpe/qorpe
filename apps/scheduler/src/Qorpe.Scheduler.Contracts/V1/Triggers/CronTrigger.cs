using System.ComponentModel.DataAnnotations;

namespace Qorpe.Scheduler.Contracts.V1.Triggers;

/// <summary>Cron trigger payload.</summary>
public sealed record CronTrigger(
    string Name,
    string Group,
    [Required] string CronExpression,
    string? TimeZone, // e.g. "Europe/Istanbul"
    DateTimeOffset? StartAtUtc = null,
    DateTimeOffset? EndAtUtc = null
) : Trigger(TriggerType.Cron, Name, Group, StartAtUtc, EndAtUtc);