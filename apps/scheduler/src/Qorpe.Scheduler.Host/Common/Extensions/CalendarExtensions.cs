using System.Globalization;

// Aliases to avoid type name collisions
using QorpeContracts = Qorpe.Scheduler.Contracts.V1.Calendars;
using QzICalendar = Quartz.ICalendar;
using QzCronCalendar = Quartz.Impl.Calendar.CronCalendar;
using QzAnnualCalendar = Quartz.Impl.Calendar.AnnualCalendar;
using QzHolidayCalendar = Quartz.Impl.Calendar.HolidayCalendar;

namespace Qorpe.Scheduler.Host.Common.Extensions;

/// <summary>Converts between API calendar DTOs and Quartz ICalendar instances.</summary>
public static class CalendarExtensions
{
    #region ToQuartz

    /// <summary>Creates a Quartz ICalendar from a Contracts.Calendar DTO.</summary>
    public static QzICalendar ToQuartz(this QorpeContracts.Calendar dto) => dto switch
    {
        QorpeContracts.CronCalendar c    => CreateCron(c),
        QorpeContracts.AnnualCalendar a  => CreateAnnual(a),
        QorpeContracts.HolidayCalendar h => CreateHoliday(h),
        _ => throw new NotSupportedException($"Unsupported calendar kind: {dto.Kind}")
    };

    private static QzICalendar CreateCron(QorpeContracts.CronCalendar dto)
    {
        var tz = !string.IsNullOrWhiteSpace(dto.TimeZoneId)
            ? TimeZoneInfo.FindSystemTimeZoneById(dto.TimeZoneId!)
            : TimeZoneInfo.Utc;

        // NOTE: Prefer contracts.CronExpression: string
        // If your contracts currently use Quartz.CronExpression, use: dto.CronExpression.CronExpression
        var expr = dto.CronExpression; // <-- should be a string in contracts

        var cal = new QzCronCalendar(expr) { TimeZone = tz };
        return cal;
    }

    private static QzICalendar CreateAnnual(QorpeContracts.AnnualCalendar dto)
    {
        var cal = new QzAnnualCalendar();
        foreach (var key in dto.ExcludedDayKeys)
        {
            // Expecting "MM-dd"
            if (!DateTime.TryParseExact(key, "MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                throw new FormatException($"Invalid Annual day key: {key} (expected MM-dd).");

            // Quartz API: 2 params (month, day)
            cal.SetDayExcluded(dt, true);
        }
        return cal;
    }

    private static QzICalendar CreateHoliday(QorpeContracts.HolidayCalendar dto)
    {
        var cal = new QzHolidayCalendar();
        foreach (var iso in dto.ExcludedDatesUtc)
        {
            if (DateTime.TryParse(iso, CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var d))
            {
                cal.AddExcludedDate(d.Date);
            }
            else
            {
                throw new FormatException($"Invalid ISO date: {iso} (expected yyyy-MM-dd).");
            }
        }
        return cal;
    }

    #endregion

    #region FromQuartz

    /// <summary>Builds a Contracts.Calendar DTO from a Quartz ICalendar.</summary>
    public static QorpeContracts.Calendar FromQuartz(QzICalendar calendar) => calendar switch
    {
        QzCronCalendar cc    => new QorpeContracts.CronCalendar(
                                    cc.CronExpression.CronExpressionString,
                                    cc.TimeZone.Id),
        QzAnnualCalendar ac  => new QorpeContracts.AnnualCalendar(GetAnnualExcluded(ac)),
        QzHolidayCalendar hc => new QorpeContracts.HolidayCalendar(
                                    hc.ExcludedDates
                                      .Select(d => d.ToString("yyyy-MM-dd"))
                                      .OrderBy(s => s)
                                      .ToArray()),
        _ => throw new NotSupportedException($"Unsupported calendar type: {calendar.GetType().Name}")
    };

    /// <summary>Collects all excluded MM-dd keys from a Quartz AnnualCalendar.</summary>
    private static IReadOnlyCollection<string> GetAnnualExcluded(QzAnnualCalendar ac)
    {
        var list = new List<string>();
        for (var m = 1; m <= 12; m++)
        {
            // 2001 is an arbitrary non-leap-year placeholder for day range
            for (var d = 1; d <= DateTime.DaysInMonth(2001, m); d++)
            {
                var dt = new DateTime(2001, m, d); // year is irrelevant
                if (ac.IsDayExcluded(dt))
                    list.Add($"{m:00}-{d:00}");
            }
        }
        return list;
    }

    #endregion
}