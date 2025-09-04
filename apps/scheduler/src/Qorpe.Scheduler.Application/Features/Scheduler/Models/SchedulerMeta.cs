namespace Qorpe.Scheduler.Application.Features.Scheduler.Models;

/// <summary>
/// Snapshot of scheduler metadata (based on Quartz.SchedulerMetaData).
/// </summary>
public sealed record SchedulerMeta
{
    public string SchedulerName { get; init; } = null!;
    public string SchedulerInstanceId { get; init; } = null!;
    public string SchedulerType { get; init; } = null!;
    public DateTimeOffset RunningSince { get; init; }
    public TimeSpan JobStoreClusterCheckinInterval { get; init; }
    public bool InStandbyMode { get; init; }
    public bool IsShutdown { get; init; }
    public bool IsJobStoreClustered { get; init; }
    public string JobStoreType { get; init; } = null!;
    public string ThreadPoolType { get; init; } = null!;
    public int ThreadPoolSize { get; init; }
}