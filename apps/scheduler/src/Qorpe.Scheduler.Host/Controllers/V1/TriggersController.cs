using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl.Matchers;
using Qorpe.Scheduler.Application.Features.Triggers;
using Qorpe.Scheduler.Contracts.V1.Triggers;
using Qorpe.Scheduler.Infrastructure.Scheduling.Jobs;
using JobKey = Quartz.JobKey;
using TriggerKey = Quartz.TriggerKey;
using ContractsJobKey = Qorpe.Scheduler.Contracts.V1.Triggers.JobKey;
using ContractsTriggerKey = Qorpe.Scheduler.Contracts.V1.Triggers.TriggerKey;

namespace Qorpe.Scheduler.Host.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("t/{tenant}/scheduler/v{version:apiVersion}/triggers")]
[Authorize(Policy = "TenantMatch")]
public sealed class TriggersController(ITriggersService svc) : ControllerBase
{
    #region Helpers

    /// <summary>Builds the standard tenant prefix (e.g., "t:acme:").</summary>
    private static string Prefix(string tenant) => $"t:{tenant}:";

    /// <summary>Scopes a group with a tenant prefix on input.</summary>
    private static string ScopeGroup(string tenant, string group)
    {
        var pref = Prefix(tenant);
        return group.StartsWith(pref, StringComparison.Ordinal) ? group : $"{pref}{group}";
    }

    /// <summary>Removes the tenant prefix from a group on output.</summary>
    private static string UnScopeGroup(string tenant, string group)
    {
        var pref = Prefix(tenant);
        return group.StartsWith(pref, StringComparison.Ordinal) ? group[pref.Length..] : group;
    }

    /// <summary>Returns true if a TriggerKey belongs to the current tenant by group prefix.</summary>
    private static bool BelongsToTenant(string tenant, TriggerKey key)
        => key.Group.StartsWith(Prefix(tenant), StringComparison.Ordinal);

    /// <summary>Builds GroupMatcher&lt;TriggerKey&gt; scoped to tenant.</summary>
    private static GroupMatcher<TriggerKey> ToTenantMatcher(string tenant, Matcher dto) => dto.Op switch
    {
        MatcherOp.GroupEquals     => GroupMatcher<TriggerKey>.GroupEquals(ScopeGroup(tenant, dto.Expr)),
        MatcherOp.GroupStartsWith => GroupMatcher<TriggerKey>.GroupStartsWith(ScopeGroup(tenant, dto.Expr)),
        MatcherOp.GroupEndsWith   => GroupMatcher<TriggerKey>.GroupEndsWith(ScopeGroup(tenant, dto.Expr)),
        MatcherOp.GroupContains   => GroupMatcher<TriggerKey>.GroupContains(ScopeGroup(tenant, dto.Expr)),
        _ => GroupMatcher<TriggerKey>.GroupEquals(ScopeGroup(tenant, dto.Expr))
    };

    /// <summary>Creates ITrigger from Cron/Simple DTO (tenant-scoped group).</summary>
    private static ITrigger BuildTrigger(string tenant, Trigger dto)
    {
        var builder = TriggerBuilder.Create()
            .WithIdentity(dto.Name, ScopeGroup(tenant, dto.Group));

        if (dto.StartAtUtc is not null) builder = builder.StartAt(dto.StartAtUtc.Value);
        if (dto.EndAtUtc   is not null) builder = builder.EndAt(dto.EndAtUtc.Value);

        return dto switch
        {
            CronTrigger cron => builder
                .WithSchedule(CronScheduleBuilder
                    .CronSchedule(cron.CronExpression)
                    .InTimeZone(cron.TimeZone is not null
                        ? TimeZoneInfo.FindSystemTimeZoneById(cron.TimeZone)
                        : TimeZoneInfo.Utc))
                .Build(),

            SimpleTrigger simple => builder
                .WithSimpleSchedule(x =>
                {
                    var interval = TimeSpan.FromSeconds(simple.RepeatIntervalSeconds);
                    if (simple.RepeatCount < 0) x.WithInterval(interval).RepeatForever();
                    else x.WithInterval(interval).WithRepeatCount(simple.RepeatCount);
                })
                .Build(),

            _ => throw new InvalidOperationException("Unknown trigger type")
        };
    }

    /// <summary>Creates IJobDetail (tenant-scoped group).</summary>
    private static IJobDetail BuildJobDetail(string tenant, string name, string group, string jobType, bool durable, bool requestsRecovery, Dictionary<string, object?>? data)
    {
        var type = Type.GetType(jobType, throwOnError: true)!;
        var jb = JobBuilder.Create(type)
                           .WithIdentity(name, ScopeGroup(tenant, group))
                           .StoreDurably(durable)
                           .RequestRecovery(requestsRecovery);

        if (data is null || data.Count <= 0) return jb.Build();
        var map = new JobDataMap();
        foreach (var (k, v) in data)
            if (v != null)
                map.Put(k, v);
        jb = jb.UsingJobData(map);

        return jb.Build();
    }

    /// <summary>Scopes Contracts TriggerKey → Quartz TriggerKey.</summary>
    private static TriggerKey Scope(string tenant, ContractsTriggerKey dto) => new(dto.Name, ScopeGroup(tenant, dto.Group));

    /// <summary>Scopes Contracts JobKey → Quartz JobKey.</summary>
    private static JobKey Scope(string tenant, ContractsJobKey dto) => new(dto.Name, ScopeGroup(tenant, dto.Group));

    #endregion

    #region Query

    /// <summary>Returns whether the given trigger group is paused (tenant-scoped).</summary>
    [HttpGet("groups/{group}/paused")]
    public async Task<ActionResult<bool>> IsTriggerGroupPaused([FromRoute] string tenant, [FromRoute] string group, CancellationToken ct)
        => Ok(await svc.IsTriggerGroupPaused(ScopeGroup(tenant, group), ct));

    /// <summary>Returns trigger group names for this tenant only.</summary>
    [HttpGet("groups")]
    public async Task<ActionResult<List<string>>> GetTriggerGroupNames([FromRoute] string tenant, CancellationToken ct)
    {
        var pref = Prefix(tenant);
        var all = await svc.GetTriggerGroupNames(ct);
        var mine = all.Where(g => g.StartsWith(pref, StringComparison.Ordinal))
                      .Select(g => UnScopeGroup(tenant, g))
                      .ToList();
        return Ok(mine);
    }

    /// <summary>Returns paused trigger groups for this tenant only.</summary>
    [HttpGet("groups/paused")]
    public async Task<ActionResult<List<string>>> GetPausedTriggerGroups([FromRoute] string tenant, CancellationToken ct)
    {
        var pref = Prefix(tenant);
        var all = await svc.GetPausedTriggerGroups(ct);
        var mine = all.Where(g => g.StartsWith(pref, StringComparison.Ordinal))
                      .Select(g => UnScopeGroup(tenant, g))
                      .ToList();
        return Ok(mine);
    }

    /// <summary>Returns triggers of a job (tenant-scoped job group; output groups are unscoped).</summary>
    [HttpGet("jobs/{jobGroup}/{jobName}/triggers")]
    public async Task<ActionResult<List<object>>> GetTriggersOfJob([FromRoute] string tenant, [FromRoute] string jobGroup, [FromRoute] string jobName, CancellationToken ct)
    {
        var jobKey = new JobKey(jobName, ScopeGroup(tenant, jobGroup));
        var list = await svc.GetTriggersOfJob(jobKey, ct);
        var mine = list
            .Where(t => BelongsToTenant(tenant, t.Key))
            .Select(t => new { t.Key.Name, Group = UnScopeGroup(tenant, t.Key.Group), t.Description, t.JobKey })
            .ToList();
        return Ok(mine);
    }

    /// <summary>Returns trigger keys by matcher (tenant-scoped matcher; output groups are unscoped).</summary>
    [HttpPost("keys/search")]
    public async Task<ActionResult<List<ContractsTriggerKey>>> GetTriggerKeys([FromRoute] string tenant, [FromBody] Matcher matcher, CancellationToken ct)
    {
        var keys = await svc.GetTriggerKeys(ToTenantMatcher(tenant, matcher), ct);
        return Ok(keys.Select(k => new ContractsTriggerKey(k.Name, UnScopeGroup(tenant, k.Group))).ToList());
    }

    /// <summary>Returns trigger by key (tenant guard; an output group is unscoped).</summary>
    [HttpGet("{group}/{name}")]
    public async Task<ActionResult<object>> GetTrigger([FromRoute] string tenant, [FromRoute] string group, [FromRoute] string name, CancellationToken ct)
    {
        var tr = await svc.GetTrigger(new TriggerKey(name, ScopeGroup(tenant, group)), ct);
        if (tr is null) return NotFound();
        if (!BelongsToTenant(tenant, tr.Key)) return Forbid();
        return Ok(new { tr.Key.Name, Group = UnScopeGroup(tenant, tr.Key.Group), tr.Description, tr.JobKey });
    }

    /// <summary>Returns trigger state by key (tenant-scoped).</summary>
    [HttpGet("{group}/{name}/state")]
    public async Task<ActionResult<string>> GetTriggerState([FromRoute] string tenant, [FromRoute] string group, [FromRoute] string name, CancellationToken ct)
        => Ok((await svc.GetTriggerState(new TriggerKey(name, ScopeGroup(tenant, group)), ct)).ToString());

    /// <summary>Checks trigger existence by key (tenant-scoped).</summary>
    [HttpGet("{group}/{name}/exists")]
    public async Task<ActionResult<bool>> CheckExists([FromRoute] string tenant, [FromRoute] string group, [FromRoute] string name, CancellationToken ct)
        => Ok(await svc.CheckExists(new TriggerKey(name, ScopeGroup(tenant, group)), ct));

    #endregion

    #region Schedule

    /// <summary>Schedules a job detail and a trigger (both tenant-scoped).</summary>
    [HttpPost("schedule/with-job")]
    public async Task<ActionResult<DateTimeOffset>> ScheduleJob([FromRoute] string tenant, [FromBody] ScheduleJobWithDetailRequest req, CancellationToken ct)
    {
        var job = BuildJobDetail(tenant, req.JobName, req.JobGroup, req.JobType, req.Durable, req.RequestsRecovery, req.Data);
        var trigger = BuildTrigger(tenant, req.Trigger);
        var when = await svc.ScheduleJob(job, trigger, ct);
        return Ok(when);
    }

    /// <summary>Schedules a trigger for an existing job key (tenant-scoped).</summary>
    [HttpPost("schedule/trigger-only")]
    public async Task<ActionResult<DateTimeOffset>> ScheduleJob([FromRoute] string tenant, [FromBody] ScheduleTriggerOnlyRequest req, CancellationToken ct)
    {
        var trigger = BuildTrigger(tenant, req.Trigger)
            .GetTriggerBuilder()
            .ForJob(Scope(tenant, req.JobKey))
            .Build();

        var when = await svc.ScheduleJob(trigger, ct);
        return Ok(when);
    }

    /// <summary>Schedules multiple jobs with their triggers (tenant-scoped).</summary>
    [HttpPost("schedule/batch")]
    public async Task<IActionResult> ScheduleJobs([FromRoute] string tenant, [FromBody] ScheduleJobsBatchRequest req, CancellationToken ct)
    {
        var dict = new Dictionary<IJobDetail, IReadOnlyCollection<ITrigger>>();
        foreach (var item in req.Items)
        {
            var job = BuildJobDetail(tenant, item.JobName, item.JobGroup, item.JobType, item.Durable, item.RequestsRecovery, item.Data);
            var triggers = item.Triggers
                .Select(dto => BuildTrigger(tenant, dto))
                .Select(t => t.GetTriggerBuilder().ForJob(job).Build())
                .ToList();

            dict[job] = triggers;
        }

        await svc.ScheduleJobs(dict, req.ReplaceExisting, ct);
        return NoContent();
    }

    /// <summary>Schedules triggers for an existing stored job (uses a durable NoOp stub; tenant-scoped).</summary>
    [HttpPost("schedule/for-job/{jobGroup}/{jobName}")]
    public async Task<IActionResult> ScheduleForJob(
        [FromRoute] string tenant,
        [FromRoute] string jobGroup,
        [FromRoute] string jobName,
        [FromBody] List<Trigger> triggers,
        [FromQuery] bool replace = false,
        CancellationToken ct = default)
    {
        var jobKey = new JobKey(jobName, ScopeGroup(tenant, jobGroup));

        var built = triggers
            .Select(dto => BuildTrigger(tenant, dto))
            .Select(t => t.GetTriggerBuilder().ForJob(jobKey).Build())
            .ToList();

        // Durable NoOp job detail with the same identity (stub).
        var jobDetail = JobBuilder.Create<NoOpJob>()
            .WithIdentity(jobKey)
            .StoreDurably()
            .Build();

        await svc.ScheduleJob(jobDetail, built, replace, ct);
        return NoContent();
    }

    #endregion

    #region Unschedule / Reschedule

    /// <summary>Unschedules a single trigger by key (tenant-scoped).</summary>
    [HttpDelete("{group}/{name}")]
    public async Task<ActionResult<bool>> UnscheduleJob([FromRoute] string tenant, [FromRoute] string group, [FromRoute] string name, CancellationToken ct)
        => Ok(await svc.UnscheduleJob(new TriggerKey(name, ScopeGroup(tenant, group)), ct));

    /// <summary>Unschedules multiple triggers by keys (tenant-scoped).</summary>
    [HttpPost("unschedule")]
    public async Task<ActionResult<bool>> UnscheduleJobs([FromRoute] string tenant, [FromBody] List<ContractsTriggerKey> keys, CancellationToken ct)
        => Ok(await svc.UnscheduleJobs(keys.Select(k => Scope(tenant, k)).ToList(), ct));

    /// <summary>Reschedules a trigger (tenant-scoped; new trigger built tenant-aware).</summary>
    [HttpPost("reschedule")]
    public async Task<ActionResult<DateTimeOffset?>> Reschedule([FromRoute] string tenant, [FromBody] RescheduleRequest req, CancellationToken ct)
    {
        var next = await svc.RescheduleJob(
            Scope(tenant, req.TriggerKey),
            BuildTrigger(tenant, req.NewTrigger),
            ct);

        return Ok(next);
    }

    #endregion

    #region Pause / Resume / Reset

    /// <summary>Pauses a single trigger (tenant-scoped; guard by prefix).</summary>
    [HttpPost("{group}/{name}/pause")]
    public async Task<IActionResult> Pause([FromRoute] string tenant, [FromRoute] string group, [FromRoute] string name, CancellationToken ct)
    {
        var key = new TriggerKey(name, ScopeGroup(tenant, group));
        if (!BelongsToTenant(tenant, key)) return Forbid();
        await svc.PauseTrigger(key, ct);
        return NoContent();
    }

    /// <summary>Pauses triggers by matcher (tenant-scoped matcher).</summary>
    [HttpPost("pause")]
    public async Task<IActionResult> Pause([FromRoute] string tenant, [FromBody] Matcher matcher, CancellationToken ct)
    {
        await svc.PauseTriggers(ToTenantMatcher(tenant, matcher), ct);
        return NoContent();
    }

    /// <summary>Resumes a single trigger (tenant-scoped; guard by prefix).</summary>
    [HttpPost("{group}/{name}/resume")]
    public async Task<IActionResult> Resume([FromRoute] string tenant, [FromRoute] string group, [FromRoute] string name, CancellationToken ct)
    {
        var key = new TriggerKey(name, ScopeGroup(tenant, group));
        if (!BelongsToTenant(tenant, key)) return Forbid();
        await svc.ResumeTrigger(key, ct);
        return NoContent();
    }

    /// <summary>Resumes triggers by matcher (tenant-scoped matcher).</summary>
    [HttpPost("resume")]
    public async Task<IActionResult> Resume([FromRoute] string tenant, [FromBody] Matcher matcher, CancellationToken ct)
    {
        await svc.ResumeTriggers(ToTenantMatcher(tenant, matcher), ct);
        return NoContent();
    }

    /// <summary>Resets trigger from the ERROR state (tenant-scoped; guard by prefix).</summary>
    [HttpPost("{group}/{name}/reset-error")]
    public async Task<IActionResult> ResetError([FromRoute] string tenant, [FromRoute] string group, [FromRoute] string name, CancellationToken ct)
    {
        var key = new TriggerKey(name, ScopeGroup(tenant, group));
        if (!BelongsToTenant(tenant, key)) return Forbid();
        await svc.ResetTriggerFromErrorState(key, ct);
        return NoContent();
    }

    #endregion
}