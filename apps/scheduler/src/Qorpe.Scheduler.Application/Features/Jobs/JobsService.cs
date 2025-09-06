using Quartz;
using Quartz.Impl.Matchers;

namespace Qorpe.Scheduler.Application.Features.Jobs;

/// <summary>
/// IScheduler-based jobs service in the Application layer (uses Quartz types directly).
/// </summary>
public sealed class JobsService(ISchedulerFactory factory) : IJobsService
{
    #region Helpers

    /// <summary>
    /// Resolves the current scheduler instance.
    /// </summary>
    private async Task<IScheduler> GetSchedulerAsync(CancellationToken ct)
        => await factory.GetScheduler(ct);

    #endregion

    #region AddJob

    /// <summary>Adds a job (store only) with a replacement flag.</summary>
    public async ValueTask AddJob(IJobDetail jobDetail, bool replace, CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).AddJob(jobDetail, replace, ct);

    /// <summary>Adds a job (store only) with an extra non-durable flag.</summary>
    public async ValueTask AddJob(IJobDetail jobDetail, bool replace, bool storeNonDurableWhileAwaitingScheduling, CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).AddJob(jobDetail, replace, storeNonDurableWhileAwaitingScheduling, ct);

    #endregion

    #region CheckExists

    /// <summary>Checks if a job exists by key.</summary>
    public async ValueTask<bool> CheckExists(JobKey jobKey, CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).CheckExists(jobKey, ct);

    #endregion

    #region DeleteJob / DeleteJobs

    /// <summary>Deletes a single job by key.</summary>
    public async ValueTask<bool> DeleteJob(JobKey jobKey, CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).DeleteJob(jobKey, ct);

    /// <summary>Deletes multiple jobs by keys.</summary>
    public async ValueTask<bool> DeleteJobs(IReadOnlyCollection<JobKey> jobKeys, CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).DeleteJobs(jobKeys, ct);

    #endregion

    #region GetCurrentlyExecutingJobs

    /// <summary>Returns currently executing job contexts.</summary>
    public async ValueTask<List<IJobExecutionContext>> GetCurrentlyExecutingJobs(CancellationToken ct = default)
        => (await (await GetSchedulerAsync(ct)).GetCurrentlyExecutingJobs(ct)).ToList();

    #endregion

    #region GetJobDetail

    /// <summary>Returns job detail by key.</summary>
    public async ValueTask<IJobDetail?> GetJobDetail(JobKey jobKey, CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).GetJobDetail(jobKey, ct);

    #endregion

    #region GetJobGroupNames

    /// <summary>Returns all job group names.</summary>
    public async ValueTask<List<string>> GetJobGroupNames(CancellationToken ct = default)
        => (await (await GetSchedulerAsync(ct)).GetJobGroupNames(ct)).ToList();

    #endregion

    #region GetJobKeys

    /// <summary>Returns job keys by matcher.</summary>
    public async ValueTask<List<JobKey>> GetJobKeys(GroupMatcher<JobKey> matcher, CancellationToken ct = default)
        => (await (await GetSchedulerAsync(ct)).GetJobKeys(matcher, ct)).ToList();

    #endregion

    #region Interrupt

    /// <summary>Interrupts a job by key.</summary>
    public async ValueTask<bool> Interrupt(JobKey jobKey, CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).Interrupt(jobKey, ct);

    /// <summary>Interrupts a job by fire instance id.</summary>
    public async ValueTask<bool> Interrupt(string fireInstanceId, CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).Interrupt(fireInstanceId, ct);

    #endregion

    #region IsJobGroupPaused

    /// <summary>Returns whether the given job group is paused.</summary>
    public async ValueTask<bool> IsJobGroupPaused(string groupName, CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).IsJobGroupPaused(groupName, ct);

    #endregion

    #region PauseJob / PauseJobs

    /// <summary>Pauses a single job.</summary>
    public async ValueTask PauseJob(JobKey jobKey, CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).PauseJob(jobKey, ct);

    /// <summary>Pauses jobs by matcher.</summary>
    public async ValueTask PauseJobs(GroupMatcher<JobKey> matcher, CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).PauseJobs(matcher, ct);

    #endregion

    #region ResumeJob / ResumeJobs

    /// <summary>Resumes a single job.</summary>
    public async ValueTask ResumeJob(JobKey jobKey, CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).ResumeJob(jobKey, ct);

    /// <summary>Resumes jobs by matcher.</summary>
    public async ValueTask ResumeJobs(GroupMatcher<JobKey> matcher, CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).ResumeJobs(matcher, ct);

    #endregion

    #region TriggerJob

    /// <summary>Triggers a job immediately.</summary>
    public async ValueTask TriggerJob(JobKey jobKey, CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).TriggerJob(jobKey, ct);

    /// <summary>Triggers a job immediately with job data.</summary>
    public async ValueTask TriggerJob(JobKey jobKey, JobDataMap data, CancellationToken ct = default)
        => await (await GetSchedulerAsync(ct)).TriggerJob(jobKey, data, ct);

    #endregion
}
