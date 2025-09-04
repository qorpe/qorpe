namespace Qorpe.Scheduler.Contracts.V1.Jobs;

public record JobDetail
{
    public string Name { get; init; } = null!;
    public string Group { get; init; } = null!;
    public string? Description { get; init; }
    public bool Durable { get; init; }
    public bool RequestsRecovery { get; init; }
    public string JobType { get; init; } = null!;
    public JobDataMap Data { get; init; } = new(); // <- typed
}