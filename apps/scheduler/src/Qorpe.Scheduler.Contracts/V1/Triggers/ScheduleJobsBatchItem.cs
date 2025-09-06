namespace Qorpe.Scheduler.Contracts.V1.Triggers;

/// <summary>Batch schedule request for ScheduleJobs overload.</summary>
public sealed record ScheduleJobsBatchItem(
    string JobName, string JobGroup, string JobType, Dictionary<string, object?>? Data,
    List<Trigger> Triggers,
    bool Durable = true,
    bool RequestsRecovery = false
);