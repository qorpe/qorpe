namespace Qorpe.Scheduler.Contracts.V1.Triggers;

/// <summary>Batch request wrapper.</summary>
public sealed record ScheduleJobsBatchRequest(List<ScheduleJobsBatchItem> Items, bool ReplaceExisting);