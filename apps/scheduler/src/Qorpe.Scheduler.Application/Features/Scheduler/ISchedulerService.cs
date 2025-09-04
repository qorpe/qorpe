using Qorpe.Scheduler.Application.Features.Scheduler.Models;

namespace Qorpe.Scheduler.Application.Features.Scheduler;

/// <summary>Controls scheduler lifecycle: start, start-delayed, standby, shutdown.</summary>
public interface ISchedulerService
{
    /// <summary>Starts the scheduler threads (IScheduler.Start).</summary>
    ValueTask StartAsync(CancellationToken ct = default);

    /// <summary>Starts the scheduler after a given delay (IScheduler.StartDelayed).</summary>
    ValueTask StartDelayedAsync(TimeSpan delay, CancellationToken ct = default);

    /// <summary>Puts scheduler into standby mode (IScheduler.Standby).</summary>
    ValueTask StandbyAsync(CancellationToken ct = default);

    /// <summary>Shuts scheduler down (IScheduler.Shutdown).</summary>
    ValueTask ShutdownAsync(bool waitForJobsToComplete, CancellationToken ct = default);
    
    /// <summary>Returns the current meta-snapshot (IScheduler.GetMetaData).</summary>
    ValueTask<SchedulerMeta> GetMetaDataAsync(CancellationToken ct = default);

    /// <summary>Returns status flags & ids (wraps Name/InstanceId/IsStarted/InStandby/IsShutdown).</summary>
    ValueTask<SchedulerStatus> GetStatusAsync(CancellationToken ct = default);

    /// <summary>Returns currently executing jobs (IScheduler.GetCurrentlyExecutingJobs).</summary>
    ValueTask<ExecutingJobs> GetCurrentlyExecutingJobsAsync(CancellationToken ct = default);
}