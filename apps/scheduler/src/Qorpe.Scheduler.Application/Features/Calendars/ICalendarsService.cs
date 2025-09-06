using Quartz;

namespace Qorpe.Scheduler.Application.Features.Calendars;

/// <summary>Manages scheduler calendars with Quartz types at the Application layer.</summary>
public interface ICalendarsService
{
    /// <summary>Adds or replaces a calendar.</summary>
    ValueTask AddCalendar(
        string name,
        ICalendar calendar,
        bool replace,
        bool updateTriggers,
        CancellationToken cancellationToken = default);

    /// <summary>Deletes a calendar by name.</summary>
    ValueTask<bool> DeleteCalendar(string name, CancellationToken cancellationToken = default);

    /// <summary>Gets a calendar by name.</summary>
    ValueTask<ICalendar?> GetCalendar(string name, CancellationToken cancellationToken = default);

    /// <summary>Lists calendar names.</summary>
    ValueTask<List<string>> GetCalendarNames(CancellationToken cancellationToken = default);
}