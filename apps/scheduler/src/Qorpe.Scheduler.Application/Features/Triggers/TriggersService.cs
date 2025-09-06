using Quartz;
using Quartz.Impl.Matchers;

namespace Qorpe.Scheduler.Application.Features.Triggers;

/// <summary>IScheduler-backed trigger service implementation.</summary>
public sealed class TriggersService(ISchedulerFactory factory) : ITriggersService
{
    #region Helpers
    
    private async Task<IScheduler> GetSchedulerAsync(CancellationToken ct) => await factory.GetScheduler(ct);
    
    #endregion

    #region Query
    
    /// <summary>Returns true if a trigger group is paused (AdoJobStore-safe).</summary>
    public async ValueTask<bool> IsTriggerGroupPaused(string groupName, CancellationToken cancellationToken = default)
    {
        var scheduler = await GetSchedulerAsync(cancellationToken);
        
        try
        {
            // Works with RAMJobStore; AdoJobStore throws NotImplementedException
            return await scheduler.IsTriggerGroupPaused(groupName, cancellationToken).ConfigureAwait(false);
        }
        catch (NotImplementedException)
        {
            // Fallback #1: check paused groups persisted by AdoJobStore (QRTZ_PAUSED_TRIGGER_GRPS)
            var pausedGroups = await scheduler.GetPausedTriggerGroups(cancellationToken).ConfigureAwait(false);
            if (pausedGroups.Contains(groupName))
                return true;

            // Fallback #2: if every trigger in the group is PAUSED, treat the group as paused
            var keys = await scheduler
                .GetTriggerKeys(GroupMatcher<TriggerKey>.GroupEquals(groupName), cancellationToken)
                .ConfigureAwait(false);

            if (keys.Count == 0) return false;

            foreach (var key in keys)
            {
                var state = await scheduler.GetTriggerState(key, cancellationToken).ConfigureAwait(false);
                if (state != TriggerState.Paused)
                    return false;
            }
            
            return true;
        }
    }

    public async ValueTask<List<string>> GetTriggerGroupNames(CancellationToken cancellationToken = default)
        => (await (await GetSchedulerAsync(cancellationToken)).GetTriggerGroupNames(cancellationToken)).ToList();

    public async ValueTask<List<string>> GetPausedTriggerGroups(CancellationToken cancellationToken = default)
        => (await (await GetSchedulerAsync(cancellationToken)).GetPausedTriggerGroups(cancellationToken)).ToList();

    public async ValueTask<List<ITrigger>> GetTriggersOfJob(JobKey jobKey, CancellationToken cancellationToken = default)
        => (await (await GetSchedulerAsync(cancellationToken)).GetTriggersOfJob(jobKey, cancellationToken)).ToList();

    public async ValueTask<List<TriggerKey>> GetTriggerKeys(GroupMatcher<TriggerKey> matcher, CancellationToken cancellationToken = default)
        => (await (await GetSchedulerAsync(cancellationToken)).GetTriggerKeys(matcher, cancellationToken)).ToList();

    public async ValueTask<ITrigger?> GetTrigger(TriggerKey triggerKey, CancellationToken cancellationToken = default)
        => await (await GetSchedulerAsync(cancellationToken)).GetTrigger(triggerKey, cancellationToken);

    public async ValueTask<TriggerState> GetTriggerState(TriggerKey triggerKey, CancellationToken cancellationToken = default)
        => await (await GetSchedulerAsync(cancellationToken)).GetTriggerState(triggerKey, cancellationToken);

    public async ValueTask<bool> CheckExists(TriggerKey triggerKey, CancellationToken cancellationToken = default)
        => await (await GetSchedulerAsync(cancellationToken)).CheckExists(triggerKey, cancellationToken);
    
    #endregion

    #region Schedule
    
    public async ValueTask<DateTimeOffset> ScheduleJob(IJobDetail jobDetail, ITrigger trigger, CancellationToken cancellationToken = default)
        => await (await GetSchedulerAsync(cancellationToken)).ScheduleJob(jobDetail, trigger, cancellationToken);

    public async ValueTask<DateTimeOffset> ScheduleJob(ITrigger trigger, CancellationToken cancellationToken = default)
        => await (await GetSchedulerAsync(cancellationToken)).ScheduleJob(trigger, cancellationToken);

    public async ValueTask ScheduleJobs(IReadOnlyDictionary<IJobDetail, IReadOnlyCollection<ITrigger>> triggersAndJobs, bool replace, CancellationToken cancellationToken = default)
        => await (await GetSchedulerAsync(cancellationToken)).ScheduleJobs(triggersAndJobs, replace, cancellationToken);

    public async ValueTask ScheduleJob(IJobDetail jobDetail, IReadOnlyCollection<ITrigger> triggersForJob, bool replace, CancellationToken cancellationToken = default)
        => await (await GetSchedulerAsync(cancellationToken)).ScheduleJob(jobDetail, triggersForJob, replace, cancellationToken);
    
    #endregion

    #region Unschedule / Reschedule
    
    public async ValueTask<bool> UnscheduleJob(TriggerKey triggerKey, CancellationToken cancellationToken = default)
        => await (await GetSchedulerAsync(cancellationToken)).UnscheduleJob(triggerKey, cancellationToken);

    public async ValueTask<bool> UnscheduleJobs(IReadOnlyCollection<TriggerKey> triggerKeys, CancellationToken cancellationToken = default)
        => await (await GetSchedulerAsync(cancellationToken)).UnscheduleJobs(triggerKeys, cancellationToken);

    public async ValueTask<DateTimeOffset?> RescheduleJob(TriggerKey triggerKey, ITrigger newTrigger, CancellationToken cancellationToken = default)
        => await (await GetSchedulerAsync(cancellationToken)).RescheduleJob(triggerKey, newTrigger, cancellationToken);
    
    #endregion

    #region Pause / Resume / Reset
    
    public async ValueTask PauseTrigger(TriggerKey triggerKey, CancellationToken cancellationToken = default)
        => await (await GetSchedulerAsync(cancellationToken)).PauseTrigger(triggerKey, cancellationToken);

    public async ValueTask PauseTriggers(GroupMatcher<TriggerKey> matcher, CancellationToken cancellationToken = default)
        => await (await GetSchedulerAsync(cancellationToken)).PauseTriggers(matcher, cancellationToken);

    public async ValueTask ResumeTrigger(TriggerKey triggerKey, CancellationToken cancellationToken = default)
        => await (await GetSchedulerAsync(cancellationToken)).ResumeTrigger(triggerKey, cancellationToken);

    public async ValueTask ResumeTriggers(GroupMatcher<TriggerKey> matcher, CancellationToken cancellationToken = default)
        => await (await GetSchedulerAsync(cancellationToken)).ResumeTriggers(matcher, cancellationToken);

    public async ValueTask ResetTriggerFromErrorState(TriggerKey triggerKey, CancellationToken cancellationToken = default)
        => await (await GetSchedulerAsync(cancellationToken)).ResetTriggerFromErrorState(triggerKey, cancellationToken);
    
    #endregion
}