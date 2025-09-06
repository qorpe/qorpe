using Quartz;
using Quartz.Impl.Matchers;

namespace Qorpe.Scheduler.Application.Features.Triggers;

public interface ITriggersService
{
    #region Query

    ValueTask<bool> IsTriggerGroupPaused(string groupName, CancellationToken cancellationToken = default);
    ValueTask<List<string>> GetTriggerGroupNames(CancellationToken cancellationToken = default);
    ValueTask<List<string>> GetPausedTriggerGroups(CancellationToken cancellationToken = default);
    ValueTask<List<ITrigger>> GetTriggersOfJob(JobKey jobKey, CancellationToken cancellationToken = default);
    ValueTask<List<TriggerKey>> GetTriggerKeys(GroupMatcher<TriggerKey> matcher, CancellationToken cancellationToken = default);
    ValueTask<ITrigger?> GetTrigger(TriggerKey triggerKey, CancellationToken cancellationToken = default);
    ValueTask<TriggerState> GetTriggerState(TriggerKey triggerKey, CancellationToken cancellationToken = default);
    ValueTask<bool> CheckExists(TriggerKey triggerKey, CancellationToken cancellationToken = default);

    #endregion

    #region Schedule

    ValueTask<DateTimeOffset> ScheduleJob(IJobDetail jobDetail, ITrigger trigger, CancellationToken cancellationToken = default);
    ValueTask<DateTimeOffset> ScheduleJob(ITrigger trigger, CancellationToken cancellationToken = default);
    ValueTask ScheduleJobs(IReadOnlyDictionary<IJobDetail, IReadOnlyCollection<ITrigger>> triggersAndJobs, bool replace, CancellationToken cancellationToken = default);
    ValueTask ScheduleJob(IJobDetail jobDetail, IReadOnlyCollection<ITrigger> triggersForJob, bool replace, CancellationToken cancellationToken = default);

    #endregion

    #region Unschedule / Reschedule

    ValueTask<bool> UnscheduleJob(TriggerKey triggerKey, CancellationToken cancellationToken = default);
    ValueTask<bool> UnscheduleJobs(IReadOnlyCollection<TriggerKey> triggerKeys, CancellationToken cancellationToken = default);
    ValueTask<DateTimeOffset?> RescheduleJob(TriggerKey triggerKey, ITrigger newTrigger, CancellationToken cancellationToken = default);
    
    #endregion

    #region Pause / Resume / Reset
    
    ValueTask PauseTrigger(TriggerKey triggerKey, CancellationToken cancellationToken = default);
    ValueTask PauseTriggers(GroupMatcher<TriggerKey> matcher, CancellationToken cancellationToken = default);
    ValueTask ResumeTrigger(TriggerKey triggerKey, CancellationToken cancellationToken = default);
    ValueTask ResumeTriggers(GroupMatcher<TriggerKey> matcher, CancellationToken cancellationToken = default);
    ValueTask ResetTriggerFromErrorState(TriggerKey triggerKey, CancellationToken cancellationToken = default);
    
    #endregion
    
}