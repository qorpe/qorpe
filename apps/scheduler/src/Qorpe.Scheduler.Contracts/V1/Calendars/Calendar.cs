using System.Text.Json.Serialization;

namespace Qorpe.Scheduler.Contracts.V1.Calendars;

/// <summary>Base DTO for calendar definitions.</summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(CronCalendar), "Cron")]
[JsonDerivedType(typeof(AnnualCalendar), "Annual")]
[JsonDerivedType(typeof(HolidayCalendar), "Holiday")]
public abstract record Calendar([property: JsonIgnore] CalendarKind Kind);