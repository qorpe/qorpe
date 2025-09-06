using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qorpe.Scheduler.Application.Features.Calendars;
using Qorpe.Scheduler.Contracts.V1.Calendars;
using Qorpe.Scheduler.Host.Common.Extensions;

namespace Qorpe.Scheduler.Host.Controllers.V1;

/// <summary>Calendars endpoints for managing Quartz calendars (tenant-scoped name prefix).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("t/{tenant}/scheduler/v{version:apiVersion}/calendars")]
[Authorize(Policy = "TenantMatch")]
public sealed class CalendarsController(ICalendarsService svc, ILogger<CalendarsController> logger) : ControllerBase
{
    #region Constants
    
    private const string NameSep = "::";
    
    #endregion

    #region Private Helpers
    
    /// <summary>Builds a tenant-scoped unique calendar name.</summary>
    private static string Scope(string tenant, string name) => $"{tenant}{NameSep}{name}";
    
    #endregion

    #region Endpoints

    /// <summary>Adds a calendar; supports replacement & updateTriggers.</summary>
    [HttpPost]
    [Authorize(Policy = "TenantMatch")]
    public async Task<IActionResult> AddCalendar(
        string tenant,
        [FromBody] AddCalendarRequest req,
        CancellationToken ct)
    {
        var scopedName = Scope(tenant, req.Name);
        var quartzCal = req.Calendar.ToQuartz();

        await svc.AddCalendar(scopedName, quartzCal, req.Replace, req.UpdateTriggers, ct);
        logger.LogInformation("Calendar added: {Name} (tenant: {Tenant}, replace: {Replace}, updateTriggers: {Update})",
            scopedName, tenant, req.Replace, req.UpdateTriggers);

        return CreatedAtAction(nameof(GetCalendar), new { tenant, name = req.Name }, null);
    }

    /// <summary>Deletes a calendar by name.</summary>
    [HttpDelete("{name}")]
    [Authorize(Policy = "TenantMatch")]
    public async Task<ActionResult<bool>> DeleteCalendar(string tenant, string name, CancellationToken ct)
    {
        var ok = await svc.DeleteCalendar(Scope(tenant, name), ct);
        return Ok(ok);
    }

    /// <summary>Gets a calendar’s definition.</summary>
    [HttpGet("{name}")]
    [Authorize(Policy = "TenantMatch")]
    public async Task<ActionResult<CalendarResponse>> GetCalendar(string tenant, string name, CancellationToken ct)
    {
        var scopedName = Scope(tenant, name);
        var cal = await svc.GetCalendar(scopedName, ct);
        if (cal is null) return NotFound();

        var dto = CalendarExtensions.FromQuartz(cal);
        return Ok(new CalendarResponse(name, dto));
    }

    /// <summary>Lists calendar names for the tenant.</summary>
    [HttpGet]
    [Authorize(Policy = "TenantMatch")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetCalendarNames(string tenant, CancellationToken ct)
    {
        var all = await svc.GetCalendarNames(ct);
        // Return tenant-local names (strip prefix)
        var local = all
            .Where(n => n.StartsWith(tenant + NameSep, StringComparison.Ordinal))
            .Select(n => n[(tenant.Length + NameSep.Length)..])
            .OrderBy(n => n)
            .ToList();

        return Ok(local);
    }
    
    #endregion
}
