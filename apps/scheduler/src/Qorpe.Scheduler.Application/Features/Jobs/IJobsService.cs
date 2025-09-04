using Quartz;
using Quartz.Impl.Matchers;

namespace Qorpe.Scheduler.Application.Features.Jobs;

/// <summary>Manages jobs: add/delete, pause/resume, fire-now, read keys/details.</summary>
public interface IJobsService
{
    /// <summary>Adds (stores) a job without triggers (IScheduler.AddJob overloads).</summary>
    ValueTask AddJobAsync(IJobDetail jobDetail, bool replace, bool storeNonDurableWhileAwaitingScheduling = false, CancellationToken ct = default);

    /// <summary>Deletes a single job (IScheduler.DeleteJob).</summary>
    ValueTask<bool> DeleteJobAsync(JobKey jobKey, CancellationToken ct = default);

    /// <summary>Deletes multiple jobs (IScheduler.DeleteJobs).</summary>
    ValueTask<bool> DeleteJobsAsync(IReadOnlyCollection<JobKey> jobKeys, CancellationToken ct = default);

    /// <summary>Fires a job immediately (IScheduler.TriggerJob overloads).</summary>
    ValueTask TriggerJobAsync(JobKey jobKey, JobDataMap? data = null, CancellationToken ct = default);

    /// <summary>Pauses a job (all of its triggers) (IScheduler.PauseJob).</summary>
    ValueTask PauseJobAsync(JobKey jobKey, CancellationToken ct = default);

    /// <summary>Resumes a job (IScheduler.ResumeJob).</summary>
    ValueTask ResumeJobAsync(JobKey jobKey, CancellationToken ct = default);

    /// <summary>Pauses jobs by matcher (IScheduler.PauseJobs).</summary>
    ValueTask PauseJobsAsync(GroupMatcher<JobKey> matcher, CancellationToken ct = default);

    /// <summary>Resumes jobs by matcher (IScheduler.ResumeJobs).</summary>
    ValueTask ResumeJobsAsync(GroupMatcher<JobKey> matcher, CancellationToken ct = default);

    /// <summary>Gets job keys by matcher (IScheduler.GetJobKeys).</summary>
    ValueTask<IReadOnlyCollection<JobKey>> GetJobKeysAsync(GroupMatcher<JobKey> matcher, CancellationToken ct = default);

    /// <summary>Gets job detail by key (IScheduler.GetJobDetail).</summary>
    ValueTask<IJobDetail?> GetJobDetailAsync(JobKey jobKey, CancellationToken ct = default);

    /// <summary>Gets triggers of a job (IScheduler.GetTriggersOfJob).</summary>
    ValueTask<IReadOnlyCollection<ITrigger>> GetTriggersOfJobAsync(JobKey jobKey, CancellationToken ct = default);

    /// <summary>Checks if job exists (IScheduler.CheckExists).</summary>
    ValueTask<bool> CheckExistsAsync(JobKey jobKey, CancellationToken ct = default);
}