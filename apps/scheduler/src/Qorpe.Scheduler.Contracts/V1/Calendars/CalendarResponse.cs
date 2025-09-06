namespace Qorpe.Scheduler.Contracts.V1.Calendars;

/// <summary>Response for a calendar read operation.</summary>
public sealed record CalendarResponse(
    string Name,
    Calendar Definition
);