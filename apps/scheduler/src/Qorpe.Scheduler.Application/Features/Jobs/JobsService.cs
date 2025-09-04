using Quartz;
using Quartz.Impl.Matchers;

namespace Qorpe.Scheduler.Application.Features.Jobs;

/// <summary>IScheduler-based jobs service in the Application layer (uses Quartz types directly).</summary>
public sealed class JobsService(ISchedulerFactory factory) : IJobsService
{
    private async Task<IScheduler> GetSchedulerAsync(CancellationToken ct) => await factory.GetScheduler(ct);

    public async ValueTask AddJobAsync(IJobDetail jobDetail, bool replace, bool storeNonDurableWhileAwaitingScheduling = false, CancellationToken ct = default)
    {
        var s = await GetSchedulerAsync(ct);
        if (storeNonDurableWhileAwaitingScheduling) await s.AddJob(jobDetail, replace, true, ct);
        else await s.AddJob(jobDetail, replace, ct);
    }

    public async ValueTask<bool> DeleteJobAsync(JobKey jobKey, CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).DeleteJob(jobKey, ct);

    public async ValueTask<bool> DeleteJobsAsync(IReadOnlyCollection<JobKey> jobKeys, CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).DeleteJobs(jobKeys, ct);

    public async ValueTask TriggerJobAsync(JobKey jobKey, JobDataMap? data = null, CancellationToken ct = default)
    {
        var s = await GetSchedulerAsync(ct);
        if (data is null) await s.TriggerJob(jobKey, ct);
        else await s.TriggerJob(jobKey, data, ct);
    }

    public async ValueTask PauseJobAsync(JobKey jobKey, CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).PauseJob(jobKey, ct);

    public async ValueTask ResumeJobAsync(JobKey jobKey, CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).ResumeJob(jobKey, ct);

    public async ValueTask PauseJobsAsync(GroupMatcher<JobKey> matcher, CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).PauseJobs(matcher, ct);

    public async ValueTask ResumeJobsAsync(GroupMatcher<JobKey> matcher, CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).ResumeJobs(matcher, ct);

    public async ValueTask<IReadOnlyCollection<JobKey>> GetJobKeysAsync(GroupMatcher<JobKey> matcher, CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).GetJobKeys(matcher, ct);

    public async ValueTask<IJobDetail?> GetJobDetailAsync(JobKey jobKey, CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).GetJobDetail(jobKey, ct);

    public async ValueTask<IReadOnlyCollection<ITrigger>> GetTriggersOfJobAsync(JobKey jobKey, CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).GetTriggersOfJob(jobKey, ct);

    public async ValueTask<bool> CheckExistsAsync(JobKey jobKey, CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).CheckExists(jobKey, ct);
}