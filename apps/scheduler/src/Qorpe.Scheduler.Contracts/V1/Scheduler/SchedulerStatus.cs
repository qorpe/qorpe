namespace Qorpe.Scheduler.Contracts.V1.Scheduler;

/// <summary>Represents scheduler status flags and identifiers.</summary>
public sealed record SchedulerStatus(
    string SchedulerName,
    string SchedulerInstanceId,
    bool IsStarted,
    bool InStandbyMode,
    bool IsShutdown
);