using System.ComponentModel.DataAnnotations;

namespace Qorpe.Scheduler.Contracts.V1.Calendars;

/// <summary>Cron-based exclusion calendar.</summary>
public sealed record CronCalendar(
    [property: Required] string CronExpression,
    string? TimeZoneId = null
) : Calendar(CalendarKind.Cron);