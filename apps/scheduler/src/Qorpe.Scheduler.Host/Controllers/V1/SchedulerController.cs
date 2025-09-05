using Asp.Versioning;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qorpe.Scheduler.Application.Features.Scheduler;
using Qorpe.Scheduler.Contracts.V1.Scheduler;

namespace Qorpe.Scheduler.Host.Controllers.V1;

/// <summary>
/// Exposes scheduler lifecycle & metadata endpoints.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("t/{tenant}/scheduler/v{version:apiVersion}/scheduler")]
[Authorize(Policy = "TenantMatch")]
public sealed class SchedulerController(ISchedulerService svc) : ControllerBase
{
    /// <summary>Returns status flags & identifiers.</summary>
    [HttpGet("status")]
    [ProducesResponseType(typeof(SchedulerStatus), StatusCodes.Status200OK)]
    public async Task<ActionResult<SchedulerStatus>> GetStatus(string tenant, CancellationToken ct)
    {
        var status = await svc.GetStatusAsync(ct);
        return Ok(status.Adapt<SchedulerStatus>());
    }

    /// <summary>Returns scheduler metadata snapshot.</summary>
    [HttpGet("meta")]
    [ProducesResponseType(typeof(SchedulerMetaData), StatusCodes.Status200OK)]
    public async Task<ActionResult<SchedulerMetaData>> GetMeta(string tenant, CancellationToken ct)
    {
        var meta = await svc.GetMetaDataAsync(ct);
        return Ok(meta.Adapt<SchedulerMetaData>());
    }

    /// <summary>Lists currently executing jobs.</summary>
    [HttpGet("executing")]
    [ProducesResponseType(typeof(IReadOnlyList<ExecutingJob>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ExecutingJob>>> GetExecuting(string tenant, CancellationToken ct)
    {
        var exec = await svc.GetCurrentlyExecutingJobsAsync(ct);
        return Ok(exec.Adapt<IReadOnlyList<ExecutingJob>>());
    }

    /// <summary>Starts the scheduler threads.</summary>
    [HttpPost("start")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Start(string tenant, CancellationToken ct)
    {
        await svc.StartAsync(ct);
        return NoContent();
    }

    /// <summary>Starts the scheduler after a given delay (seconds).</summary>
    [HttpPost("start-delayed")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> StartDelayed(string tenant, [FromQuery] int delaySeconds, CancellationToken ct)
    {
        await svc.StartDelayedAsync(TimeSpan.FromSeconds(delaySeconds), ct);
        return NoContent();
    }

    /// <summary>Puts scheduler into standby mode.</summary>
    [HttpPost("standby")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Standby(string tenant, CancellationToken ct)
    {
        await svc.StandbyAsync(ct);
        return NoContent();
    }

    /// <summary>Shuts scheduler down; optionally waits for running jobs.</summary>
    [HttpPost("shutdown")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Shutdown(string tenant, [FromQuery] bool? waitForJobsToComplete, CancellationToken ct)
    {
        await svc.ShutdownAsync(waitForJobsToComplete, ct);
        return NoContent();
    }
    
    /// <summary>Pause all triggers.</summary>
    [HttpPost("pause-all")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> PauseAll(string tenant, CancellationToken ct)
    {
        await svc.PauseAllAsync(ct);
        return NoContent();
    }

    /// <summary>Resume all triggers.</summary>
    [HttpPost("resume-all")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ResumeAll(string tenant, CancellationToken ct)
    {
        await svc.ResumeAllAsync(ct);
        return NoContent();
    }

    /// <summary>Clear all jobs/triggers.</summary>
    [HttpPost("clear")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Clear(string tenant, CancellationToken ct)
    {
        await svc.ClearAsync(ct);
        return NoContent();
    }
}