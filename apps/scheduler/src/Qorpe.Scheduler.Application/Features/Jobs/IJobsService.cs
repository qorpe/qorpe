using Quartz;
using Quartz.Impl.Matchers;

namespace Qorpe.Scheduler.Application.Features.Jobs;

/// <summary>Manages jobs: add/delete, pause/resume, fire-now, read keys/details.</summary>
public interface IJobsService
{
    /// <summary>Adds a job (store only) with a replacement flag.</summary>
    ValueTask AddJob(IJobDetail jobDetail, bool replace, CancellationToken ct = default);

    /// <summary>Adds a job (store only) with an extra non-durable flag.</summary>
    ValueTask AddJob(IJobDetail jobDetail, bool replace, bool storeNonDurableWhileAwaitingScheduling, CancellationToken ct = default);

    /// <summary>Checks if a job exists by key.</summary>
    ValueTask<bool> CheckExists(JobKey jobKey, CancellationToken ct = default);

    /// <summary>Deletes a single job by key.</summary>
    ValueTask<bool> DeleteJob(JobKey jobKey, CancellationToken ct = default);

    /// <summary>Deletes multiple jobs by keys.</summary>
    ValueTask<bool> DeleteJobs(IReadOnlyCollection<JobKey> jobKeys, CancellationToken ct = default);

    /// <summary>Returns currently executing job contexts.</summary>
    ValueTask<List<IJobExecutionContext>> GetCurrentlyExecutingJobs(CancellationToken ct = default);

    /// <summary>Returns job detail by key.</summary>
    ValueTask<IJobDetail?> GetJobDetail(JobKey jobKey, CancellationToken ct = default);

    /// <summary>Returns all job group names.</summary>
    ValueTask<List<string>> GetJobGroupNames(CancellationToken ct = default);

    /// <summary>Returns job keys by matcher.</summary>
    ValueTask<List<JobKey>> GetJobKeys(GroupMatcher<JobKey> matcher, CancellationToken ct = default);

    /// <summary>Interrupts a job by key.</summary>
    ValueTask<bool> Interrupt(JobKey jobKey, CancellationToken ct = default);

    /// <summary>Interrupts a job by fire instance id.</summary>
    ValueTask<bool> Interrupt(string fireInstanceId, CancellationToken ct = default);

    /// <summary>Returns whether the given job group is paused.</summary>
    ValueTask<bool> IsJobGroupPaused(string groupName, CancellationToken ct = default);

    /// <summary>Pauses a single job.</summary>
    ValueTask PauseJob(JobKey jobKey, CancellationToken ct = default);

    /// <summary>Pauses jobs by matcher.</summary>
    ValueTask PauseJobs(GroupMatcher<JobKey> matcher, CancellationToken ct = default);

    /// <summary>Resumes a single job.</summary>
    ValueTask ResumeJob(JobKey jobKey, CancellationToken ct = default);

    /// <summary>Resumes jobs by matcher.</summary>
    ValueTask ResumeJobs(GroupMatcher<JobKey> matcher, CancellationToken ct = default);

    /// <summary>Triggers a job immediately.</summary>
    ValueTask TriggerJob(JobKey jobKey, CancellationToken ct = default);

    /// <summary>Triggers a job immediately with job data.</summary>
    ValueTask TriggerJob(JobKey jobKey, JobDataMap data, CancellationToken ct = default);
}