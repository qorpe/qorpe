namespace Qorpe.Scheduler.Contracts.V1.Jobs;

public record JobDetail(JobKey JobKey, string? DetailDescription, string JobTypeAssemblyQualifiedName, bool DetailDurable, bool DetailRequestsRecovery, Dictionary<string, object> ToDictionary)
{
    public JobKey Key { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Group { get; init; } = null!;
    public string? Description { get; init; }
    public bool Durable { get; init; }
    public bool RequestsRecovery { get; init; }
    public string JobType { get; init; } = null!;
    public JobDataMap JobDataMap { get; init; } = new(); // <- typed
}