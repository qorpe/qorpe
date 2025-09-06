using Quartz;

namespace Qorpe.Scheduler.Application.Features.Calendars;

/// <summary>IScheduler-based calendars service in the Application layer (uses Quartz types directly).</summary>
public sealed class CalendarsService(ISchedulerFactory factory) : ICalendarsService
{
    #region Constructors
    
    private async Task<IScheduler> GetSchedulerAsync(CancellationToken ct) => await factory.GetScheduler(ct);
    
    #endregion

    #region Public Methods
    
    public async ValueTask AddCalendar(
        string name,
        ICalendar calendar,
        bool replace,
        bool updateTriggers,
        CancellationToken cancellationToken = default)
        => await (await GetSchedulerAsync(cancellationToken))
            .AddCalendar(name, calendar, replace, updateTriggers, cancellationToken);

    public async ValueTask<bool> DeleteCalendar(string name, CancellationToken cancellationToken = default)
        => await (await GetSchedulerAsync(cancellationToken)).DeleteCalendar(name, cancellationToken);

    public async ValueTask<ICalendar?> GetCalendar(string name, CancellationToken cancellationToken = default)
        => await (await GetSchedulerAsync(cancellationToken)).GetCalendar(name, cancellationToken);

    public async ValueTask<List<string>> GetCalendarNames(CancellationToken cancellationToken = default)
        => (await (await GetSchedulerAsync(cancellationToken)).GetCalendarNames(cancellationToken)).ToList();
    
    #endregion
}