namespace Qorpe.Scheduler.Contracts.V1.Triggers;

/// <summary>Reschedule request.</summary>
public sealed record RescheduleRequest(TriggerKey TriggerKey, Trigger NewTrigger);