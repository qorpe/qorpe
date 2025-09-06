using System.ComponentModel.DataAnnotations;

namespace Qorpe.Scheduler.Contracts.V1.Calendars;

/// <summary>Holiday calendar to exclude exact dates (UTC yyyy-MM-dd).</summary>
public sealed record HolidayCalendar(
    [property: Required] IReadOnlyCollection<string> ExcludedDatesUtc // e.g., "2025-01-01"
) : Calendar(CalendarKind.Holiday);