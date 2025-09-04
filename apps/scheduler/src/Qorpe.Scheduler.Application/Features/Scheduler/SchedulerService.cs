using Mapster;
using Qorpe.Scheduler.Application.Features.Scheduler.Models;
using Quartz;

namespace Qorpe.Scheduler.Application.Features.Scheduler;

/// <summary>ISchedulerService backed by Quartz in the Application layer.</summary>
public sealed class SchedulerService(ISchedulerFactory schedulerFactory) : ISchedulerService
{
    private async Task<IScheduler> GetSchedulerAsync(CancellationToken ct) => await schedulerFactory.GetScheduler(ct);

    public async ValueTask StartAsync(CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).Start(ct);

    public async ValueTask StartDelayedAsync(TimeSpan delay, CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).StartDelayed(delay, ct);

    public async ValueTask StandbyAsync(CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).Standby(ct);

    public async ValueTask ShutdownAsync(bool waitForJobsToComplete, CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).Shutdown(waitForJobsToComplete, ct);

    public async ValueTask<SchedulerMeta> GetMetaDataAsync(CancellationToken ct = default)
        => (await (await GetSchedulerAsync(ct)).GetMetaData(ct)).Adapt<SchedulerMeta>();

    public async ValueTask<SchedulerStatus> GetStatusAsync(CancellationToken ct = default)
    {
        var s = await GetSchedulerAsync(ct);
        return new SchedulerStatus(s.SchedulerName, s.SchedulerInstanceId, s.IsStarted, s.InStandbyMode, s.IsShutdown);
    }

    public async ValueTask<ExecutingJobs> GetCurrentlyExecutingJobsAsync(CancellationToken ct = default)
    {
        var items = await (await GetSchedulerAsync(ct)).GetCurrentlyExecutingJobs(ct);
        var mapped = items.Adapt<IReadOnlyList<ExecutingJob>>();
        return new ExecutingJobs(mapped);
    }
}