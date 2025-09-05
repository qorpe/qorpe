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

    public async ValueTask ShutdownAsync(bool? waitForJobsToComplete = null, CancellationToken ct = default)
    {
        var sch = await GetSchedulerAsync(ct);
        if (waitForJobsToComplete is null)
            await sch.Shutdown(ct);
        else
            await sch.Shutdown(waitForJobsToComplete.Value, ct);
    }
    
    public async ValueTask<SchedulerMetaData> GetMetaDataAsync(CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).GetMetaData(ct);

    public async ValueTask<SchedulerStatus> GetStatusAsync(CancellationToken ct = default)
    {
        var s = await GetSchedulerAsync(ct);
        return new SchedulerStatus(s.SchedulerName, s.SchedulerInstanceId, s.IsStarted, s.InStandbyMode, s.IsShutdown);
    }

    public async ValueTask<IReadOnlyCollection<IJobExecutionContext>> GetCurrentlyExecutingJobsAsync(CancellationToken ct = default) 
        => await (await GetSchedulerAsync(ct)).GetCurrentlyExecutingJobs(ct);
    
    public async ValueTask PauseAllAsync(CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).PauseAll(ct);

    public async ValueTask ResumeAllAsync(CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).ResumeAll(ct);

    public async ValueTask ClearAsync(CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).Clear(ct);
}