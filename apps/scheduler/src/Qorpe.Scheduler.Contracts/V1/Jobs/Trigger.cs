namespace Qorpe.Scheduler.Contracts.V1.Jobs;

public record Trigger
{
    public string Name { get; init; } = null!;
    public string Group { get; init; } = null!;
    public string Type { get; init; } = null!;
    public DateTimeOffset? NextFireTimeUtc { get; init; }
    public DateTimeOffset? PreviousFireTimeUtc { get; init; }
    public DateTimeOffset? StartAtUtc { get; init; }
    public DateTimeOffset? EndAtUtc { get; init; }
    public string? CalendarName { get; init; }
    public string? MisfireInstruction { get; init; }
}