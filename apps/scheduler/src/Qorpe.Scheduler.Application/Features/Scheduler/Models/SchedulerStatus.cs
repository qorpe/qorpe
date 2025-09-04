namespace Qorpe.Scheduler.Application.Features.Scheduler.Models;

/// <summary>Represents scheduler status flags and identifiers.</summary>
public sealed record SchedulerStatus(
    string SchedulerName,
    string SchedulerInstanceId,
    bool IsStarted,
    bool InStandbyMode,
    bool IsShutdown
);