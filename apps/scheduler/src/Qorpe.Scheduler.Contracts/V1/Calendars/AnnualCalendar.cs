using System.ComponentModel.DataAnnotations;

namespace Qorpe.Scheduler.Contracts.V1.Calendars;

/// <summary>Annual calendar to exclude specific month/day pairs (MM-dd).</summary>
public sealed record AnnualCalendar(
    [property: Required] IReadOnlyCollection<string> ExcludedDayKeys // e.g., "01-01","12-31"
) : Calendar(CalendarKind.Annual);