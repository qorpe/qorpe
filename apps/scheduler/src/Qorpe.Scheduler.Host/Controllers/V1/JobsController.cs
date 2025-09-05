using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qorpe.Scheduler.Application.Features.Jobs;
using Qorpe.Scheduler.Contracts.V1.Jobs;
using Quartz;
using Quartz.Impl.Matchers;
using JobDataMap = Quartz.JobDataMap;
using JobKey = Quartz.JobKey;

namespace Qorpe.Scheduler.Host.Controllers.V1;

/// <summary>Exposes job management endpoints (Application returns Quartz types).</summary>
[ApiController]
[Route("t/{tenant}/scheduler/v{version:apiVersion}/jobs")]
[Authorize(Policy = "TenantMatch")]
public sealed class JobsController(IJobsService svc) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddJob(string tenant, [FromBody] AddJobRequest req, CancellationToken ct)
    {
        var group = $"tenant:{tenant}";
        var jobType = Type.GetType(req.JobType, throwOnError: true)!;

        var jb = JobBuilder.Create(jobType)
                           .WithIdentity(req.Name, group)
                           .WithDescription(req.Description)
                           .StoreDurably(req.Durable)
                           .RequestRecovery(req.RequestsRecovery);

        if (req.Data is { Count: > 0 })
        {
            var map = new JobDataMap();
            foreach (var (k, v) in req.Data)
                if (v != null)
                    map[k] = v;
            jb = jb.UsingJobData(map);
        }

        await svc.AddJobAsync(jb.Build(), replace: true, ct: ct);
        return NoContent();
    }

    [HttpDelete("{name}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> DeleteJob(string tenant, string name, CancellationToken ct)
        => Ok(await svc.DeleteJobAsync(new JobKey(name, $"tenant:{tenant}"), ct));

    [HttpPost("bulk-delete")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> BulkDelete([FromBody] List<JobKey> req, CancellationToken ct)
    {
        var keys = req.Select(k => new JobKey(k.Name, k.Group)).ToList();
        return Ok(await svc.DeleteJobsAsync(keys, ct));
    }

    [HttpPost("{name}:runNow")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RunNow(string tenant, string name, [FromBody] JobDataMap? req, CancellationToken ct)
    {
        var key = new JobKey(name, $"tenant:{tenant}");
        JobDataMap? map = null;
        if (req is { Count: > 0 })
        {
            map = new JobDataMap();
            foreach (var (k, v) in req) map[k] = v;
        }
        await svc.TriggerJobAsync(key, map, ct);
        return NoContent();
    }

    [HttpPost("{name}:pause")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Pause(string tenant, string name, CancellationToken ct)
    {
        await svc.PauseJobAsync(new JobKey(name, $"tenant:{tenant}"), ct);
        return NoContent();
    }

    [HttpPost("{name}:resume")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Resume(string tenant, string name, CancellationToken ct)
    {
        await svc.ResumeJobAsync(new JobKey(name, $"tenant:{tenant}"), ct);
        return NoContent();
    }

    [HttpPost("group:pause")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> PauseGroup(string tenant, [FromBody] GroupMatcherRequest req, CancellationToken ct)
    {
        var matcher = BuildMatcher(req.Mode, req.Pattern);
        await svc.PauseJobsAsync(matcher, ct);
        return NoContent();
    }

    [HttpPost("group:resume")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ResumeGroup(string tenant, [FromBody] GroupMatcherRequest req, CancellationToken ct)
    {
        var matcher = BuildMatcher(req.Mode, req.Pattern);
        await svc.ResumeJobsAsync(matcher, ct);
        return NoContent();
    }

    [HttpGet("keys")]
    [ProducesResponseType(typeof(JobKeyList), StatusCodes.Status200OK)]
    public async Task<ActionResult<JobKeyList>> Keys(string tenant, [FromQuery] string mode, [FromQuery] string pattern, CancellationToken ct)
    {
        var keys = await svc.GetJobKeysAsync(BuildMatcher(mode, pattern), ct);
        return Ok(keys.Adapt<JobKeyList>());
    }

    [HttpGet("{name}")]
    [ProducesResponseType(typeof(JobDetail), StatusCodes.Status200OK)]
    public async Task<ActionResult<JobDetail>> Detail(string tenant, string name, CancellationToken ct)
    {
        var jd = await svc.GetJobDetailAsync(new JobKey(name, $"tenant:{tenant}"), ct);
        return Ok(jd?.Adapt<JobDetail>());
    }

    [HttpGet("{name}/triggers")]
    [ProducesResponseType(typeof(List<Trigger>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Trigger>>> Triggers(string tenant, string name, CancellationToken ct)
    {
        var list = await svc.GetTriggersOfJobAsync(new JobKey(name, $"tenant:{tenant}"), ct);
        return Ok(list.Adapt<List<Trigger>>());
    }

    [HttpGet("{name}/exists")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> Exists(string tenant, string name, CancellationToken ct)
        => Ok(await svc.CheckExistsAsync(new JobKey(name, $"tenant:{tenant}"), ct));

    private static GroupMatcher<JobKey> BuildMatcher(string mode, string pattern)
        => mode switch
        {
            "Equals"     => GroupMatcher<JobKey>.GroupEquals(pattern),
            "StartsWith" => GroupMatcher<JobKey>.GroupStartsWith(pattern),
            "EndsWith"   => GroupMatcher<JobKey>.GroupEndsWith(pattern),
            "Contains"   => GroupMatcher<JobKey>.GroupContains(pattern),
            _            => GroupMatcher<JobKey>.GroupEquals(pattern)
        };
}