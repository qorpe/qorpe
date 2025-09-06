namespace Qorpe.Scheduler.Contracts.V1.Triggers;

/// <summary>Schedule: only trigger (for an existing job key).</summary>
public sealed record ScheduleTriggerOnlyRequest(
    JobKey JobKey,
    Trigger Trigger
);