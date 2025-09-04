namespace Qorpe.Scheduler.Contracts.V1.Scheduler;

/// <summary>
/// Snapshot of a currently executing job (based on Quartz.IJobExecutionContext).
/// </summary>
public sealed record ExecutingJob
{
    public string JobName { get; init; } = null!;
    public string JobGroup { get; init; } = null!;
    public string FireInstanceId { get; init; } = null!;
    public DateTimeOffset FireTimeUtc { get; init; }
    public DateTimeOffset? ScheduledFireTimeUtc { get; init; }
    public DateTimeOffset? PreviousFireTimeUtc { get; init; }
    public DateTimeOffset? NextFireTimeUtc { get; init; }
    public TimeSpan RunTime { get; init; }
    public string TriggerName { get; init; } = null!;
    public string TriggerGroup { get; init; } = null!;
    public IDictionary<string, object?> MergedJobDataMap { get; init; } 
        = new Dictionary<string, object?>();
}