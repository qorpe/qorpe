using System.ComponentModel.DataAnnotations;
using Asp.Versioning;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qorpe.Scheduler.Application.Features.Jobs;
using Qorpe.Scheduler.Contracts.V1.Jobs;
using Quartz;
using Quartz.Impl.Matchers;

// Aliases to avoid name clashes
using QuartzJobDataMap = Quartz.JobDataMap;
using QuartzJobKey = Quartz.JobKey;
using ContractsJobKey = Qorpe.Scheduler.Contracts.V1.Jobs.JobKey;

namespace Qorpe.Scheduler.Host.Controllers.V1;

/// <summary>Exposes job management endpoints (Application uses Quartz types internally).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("t/{tenant}/scheduler/v{version:apiVersion}/jobs")]
[Authorize(Policy = "TenantMatch")]
public sealed class JobsController(IJobsService svc) : ControllerBase
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

    /// <summary>Scopes a Quartz JobKey with tenant group prefix.</summary>
    private static QuartzJobKey Scope(string tenant, QuartzJobKey key)
        => new(key.Name, ScopeGroup(tenant, key.Group));

    /// <summary>Builds a tenant-scoped GroupMatcher for JobKey groups.</summary>
    private static GroupMatcher<QuartzJobKey> BuildMatcher(string tenant, string @operator, string compareTo)
    {
        var scoped = ScopeGroup(tenant, compareTo);
        return @operator.ToUpperInvariant() switch
        {
            "EQUALS"      => GroupMatcher<QuartzJobKey>.GroupEquals(scoped),
            "STARTS_WITH" => GroupMatcher<QuartzJobKey>.GroupStartsWith(scoped),
            "ENDS_WITH"   => GroupMatcher<QuartzJobKey>.GroupEndsWith(scoped),
            "CONTAINS"    => GroupMatcher<QuartzJobKey>.GroupContains(scoped),
            _ => throw new ValidationException("operator must be one of: EQUALS, STARTS_WITH, ENDS_WITH, CONTAINS")
        };
    }

    #endregion

    #region AddJob

    /// <summary>Adds (stores) a job definition using Quartz JobBuilder.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddJob(
        [FromRoute] string tenant,
        [FromBody] AddJobRequest req,
        [FromQuery] AddJobFlags flags,
        CancellationToken ct)
    {
        // Validate JobType
        var jobType = Type.GetType(req.JobType, throwOnError: false);
        if (jobType is null || !typeof(IJob).IsAssignableFrom(jobType))
            throw new ValidationException("JobType must implement Quartz.IJob (use assembly-qualified name).");

        // Build Quartz JobDataMap from a type-safe contract
        var jdm = new QuartzJobDataMap();
        if (req.JobDataMap is not null)
        {
            foreach (var e in req.JobDataMap.Strings)  jdm[e.Key] = e.Value;
            foreach (var e in req.JobDataMap.Ints)     jdm[e.Key] = e.Value;
            foreach (var e in req.JobDataMap.Longs)    jdm[e.Key] = e.Value;
            foreach (var e in req.JobDataMap.Bools)    jdm[e.Key] = e.Value;
            foreach (var e in req.JobDataMap.Floats)   jdm[e.Key] = e.Value;
            foreach (var e in req.JobDataMap.Doubles)  jdm[e.Key] = e.Value;
            foreach (var e in req.JobDataMap.Decimals) jdm[e.Key] = e.Value;
            foreach (var e in req.JobDataMap.Dates)    jdm[e.Key] = e.Value;
            foreach (var e in req.JobDataMap.Bytes)    jdm[e.Key] = e.Value;
        }

        // Build IJobDetail
        var builder = JobBuilder.Create(jobType)
            .WithIdentity(req.Name, ScopeGroup(tenant, req.Group))
            .WithDescription(req.Description)
            .RequestRecovery(req.RequestsRecovery);

        builder = req.Durable ? builder.StoreDurably() : builder.StoreDurably(false);
        if (jdm.Count > 0) builder = builder.UsingJobData(jdm);

        var detail = builder.Build();

        // Call service using Quartz types
        if (flags.StoreNonDurableWhileAwaitingScheduling is null)
            await svc.AddJob(detail, flags.Replace, ct);
        else
            await svc.AddJob(detail, flags.Replace, flags.StoreNonDurableWhileAwaitingScheduling.Value, ct);

        return NoContent();
    }

    #endregion

    #region CheckExists

    /// <summary>Checks whether a job exists by key.</summary>
    [HttpGet("{group}/{name}/exists")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> CheckExists(
        [FromRoute] string tenant,
        [FromRoute] string group,
        [FromRoute] string name,
        CancellationToken ct)
        => Ok(await svc.CheckExists(Scope(tenant, new QuartzJobKey(name, group)), ct));

    #endregion

    #region DeleteJob / DeleteJobs

    /// <summary>Deletes a single job by key.</summary>
    [HttpDelete("{group}/{name}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> DeleteJob(
        [FromRoute] string tenant,
        [FromRoute] string group,
        [FromRoute] string name,
        CancellationToken ct)
        => Ok(await svc.DeleteJob(Scope(tenant, new QuartzJobKey(name, group)), ct));

    /// <summary>Deletes multiple jobs by keys.</summary>
    [HttpPost("delete-batch")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> DeleteJobs(
        [FromRoute] string tenant,
        [FromBody] IReadOnlyCollection<ContractsJobKey> req,
        CancellationToken ct)
    {
        var keys = req.Select(k => Scope(tenant, new QuartzJobKey(k.Name, k.Group))).ToList();
        return Ok(await svc.DeleteJobs(keys, ct));
    }

    #endregion

    #region GetCurrentlyExecutingJobs

    /// <summary>Returns currently executing jobs for this tenant (scoped by group prefix).</summary>
    [HttpGet("executing")]
    [ProducesResponseType(typeof(List<JobExecutionContext>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<JobExecutionContext>>> GetCurrentlyExecutingJobs(
        [FromRoute] string tenant,
        CancellationToken ct)
    {
        var pref = Prefix(tenant);
        var list = await svc.GetCurrentlyExecutingJobs(ct);

        // Map to a thin response (no extra Mapster config required)
        var res = list
            .Where(x => x.JobDetail.Key.Group.StartsWith(pref, StringComparison.Ordinal))
            .Select(x => new JobExecutionContext(
                FireInstanceId: x.FireInstanceId,
                FireTimeUtc: x.FireTimeUtc,
                JobDetailKey: new ContractsJobKey(x.JobDetail.Key.Name, UnScopeGroup(tenant, x.JobDetail.Key.Group)),
                JobDetailDescription: x.JobDetail.Description,
                Trigger: x.Trigger.Adapt<Trigger>()
            ))
            .ToList();

        return Ok(res);
    }

    #endregion

    #region GetJobDetail

    /// <summary>Returns job detail by key.</summary>
    [HttpGet("{group}/{name}")]
    [ProducesResponseType(typeof(JobDetail), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JobDetail>> GetJobDetail(
        [FromRoute] string tenant,
        [FromRoute] string group,
        [FromRoute] string name,
        CancellationToken ct)
    {
        var detail = await svc.GetJobDetail(Scope(tenant, new QuartzJobKey(name, group)), ct);
        if (detail is null) return NotFound();

        var res = new JobDetail(
            new ContractsJobKey(detail.Key.Name, UnScopeGroup(tenant, detail.Key.Group)),
            detail.Description,
            detail.JobType.AssemblyQualifiedName ?? detail.JobType.FullName ?? detail.JobType.Name,
            detail.Durable,
            detail.RequestsRecovery,
            detail.JobDataMap.WrappedMap.ToDictionary(kv => kv.Key, kv => kv.Value)
        );
        return Ok(res);
    }

    #endregion

    #region GetJobGroupNames

    /// <summary>Returns job group names for this tenant only.</summary>
    [HttpGet("groups")]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<string>>> GetJobGroupNames(
        [FromRoute] string tenant,
        CancellationToken ct)
    {
        var pref = Prefix(tenant);
        var all = await svc.GetJobGroupNames(ct);
        var mine = all
            .Where(g => g.StartsWith(pref, StringComparison.Ordinal))
            .Select(g => UnScopeGroup(tenant, g))
            .ToList();
        return Ok(mine);
    }

    #endregion

    #region GetJobKeys

    /// <summary>Returns job keys by group matcher for this tenant.</summary>
    [HttpGet("keys")]
    [ProducesResponseType(typeof(List<ContractsJobKey>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ContractsJobKey>>> GetJobKeys(
        [FromRoute] string tenant,
        [FromQuery] string @operator,
        [FromQuery] string compareTo,
        CancellationToken ct)
    {
        var keys = await svc.GetJobKeys(BuildMatcher(tenant, @operator, compareTo), ct);
        var res = keys.Select(k => new ContractsJobKey(k.Name, UnScopeGroup(tenant, k.Group))).ToList();
        return Ok(res);
    }

    #endregion

    #region Interrupt (by key) / InterruptByFireId

    /// <summary>Interrupts a job by key.</summary>
    [HttpPost("{group}/{name}/interrupt")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> Interrupt(
        [FromRoute] string tenant,
        [FromRoute] string group,
        [FromRoute] string name,
        CancellationToken ct)
        => Ok(await svc.Interrupt(Scope(tenant, new QuartzJobKey(name, group)), ct));

    /// <summary>Interrupts a job execution by its fire instance id (tenant-safe).</summary>
    public sealed record InterruptByFireIdRequest(string FireInstanceId);

    [HttpPost("interrupt-by-fire-id")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> InterruptByFireId(
        [FromRoute] string tenant,
        [FromBody] InterruptByFireIdRequest req,
        CancellationToken ct)
    {
        // Optional guard: ensure fireId belongs to this tenant
        var pref = Prefix(tenant);
        var executing = await svc.GetCurrentlyExecutingJobs(ct);
        var mine = executing.Any(x => x.FireInstanceId == req.FireInstanceId &&
                                      x.JobDetail.Key.Group.StartsWith(pref, StringComparison.Ordinal));

        return !mine ? Ok(false) : Ok(await svc.Interrupt(req.FireInstanceId, ct));
    }

    #endregion

    #region IsJobGroupPaused

    /// <summary>Returns whether the given job group is paused.</summary>
    [HttpGet("groups/{group}/paused")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> IsJobGroupPaused(
        [FromRoute] string tenant,
        [FromRoute] string group,
        CancellationToken ct)
        => Ok(await svc.IsJobGroupPaused(ScopeGroup(tenant, group), ct));

    #endregion

    #region PauseJob / PauseJobs

    /// <summary>Pauses a single job.</summary>
    [HttpPost("{group}/{name}/pause")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> PauseJob(
        [FromRoute] string tenant,
        [FromRoute] string group,
        [FromRoute] string name,
        CancellationToken ct)
    {
        await svc.PauseJob(Scope(tenant, new QuartzJobKey(name, group)), ct);
        return NoContent();
    }

    /// <summary>Pauses jobs by group matcher.</summary>
    [HttpPost("pause-by")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> PauseJobs(
        [FromRoute] string tenant,
        [FromBody] GroupMatcherRequest matcher,
        CancellationToken ct)
    {
        await svc.PauseJobs(BuildMatcher(tenant, matcher.Operator, matcher.CompareTo), ct);
        return NoContent();
    }

    #endregion

    #region ResumeJob / ResumeJobs

    /// <summary>Resumes a single job.</summary>
    [HttpPost("{group}/{name}/resume")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ResumeJob(
        [FromRoute] string tenant,
        [FromRoute] string group,
        [FromRoute] string name,
        CancellationToken ct)
    {
        await svc.ResumeJob(Scope(tenant, new QuartzJobKey(name, group)), ct);
        return NoContent();
    }

    /// <summary>Resumes jobs by group matcher.</summary>
    [HttpPost("resume-by")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ResumeJobs(
        [FromRoute] string tenant,
        [FromBody] GroupMatcherRequest matcher,
        CancellationToken ct)
    {
        await svc.ResumeJobs(BuildMatcher(tenant, matcher.Operator, matcher.CompareTo), ct);
        return NoContent();
    }

    #endregion

    #region TriggerJob

    /// <summary>Triggers a job immediately; optional job data can be supplied.</summary>
    public sealed record TriggerJobRequest(IDictionary<string, object?>? JobDataMap);

    [HttpPost("{group}/{name}/trigger")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> TriggerJob(
        [FromRoute] string tenant,
        [FromRoute] string group,
        [FromRoute] string name,
        [FromBody] TriggerJobRequest req,
        CancellationToken ct)
    {
        var key = Scope(tenant, new QuartzJobKey(name, group));
        if (req.JobDataMap is null || req.JobDataMap.Count == 0)
        {
            await svc.TriggerJob(key, ct);
        }
        else
        {
            var jdm = new QuartzJobDataMap();
            foreach (var kv in req.JobDataMap)
                if (kv.Value is not null)
                    jdm[kv.Key] = kv.Value;

            await svc.TriggerJob(key, jdm, ct);
        }
        return Accepted();
    }

    #endregion
}