namespace Qorpe.Scheduler.Contracts.V1.Triggers;

/// <summary>Simple trigger payload.</summary>
public sealed record SimpleTrigger(
    string Name,
    string Group,
    int RepeatCount,              // -1 = repeat forever
    int RepeatIntervalSeconds,    // >0
    DateTimeOffset? StartAtUtc = null,
    DateTimeOffset? EndAtUtc = null
) : Trigger(TriggerType.Simple, Name, Group, StartAtUtc, EndAtUtc);