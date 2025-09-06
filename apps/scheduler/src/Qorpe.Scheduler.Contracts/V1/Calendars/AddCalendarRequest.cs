using System.ComponentModel.DataAnnotations;

namespace Qorpe.Scheduler.Contracts.V1.Calendars;

/// <summary>Request body for adding a calendar.</summary>
public sealed record AddCalendarRequest(
    [Required] string Name,
    [Required] Calendar Calendar,
    bool Replace = false,
    bool UpdateTriggers = false
);