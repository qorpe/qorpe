namespace Qorpe.Scheduler.Contracts.V1.Jobs;

public record AddJobRequest(
    string Name,
    string? Description,
    string JobType,
    bool Durable = true,
    bool RequestsRecovery = true,
    Dictionary<string, object?>? Data = null
);