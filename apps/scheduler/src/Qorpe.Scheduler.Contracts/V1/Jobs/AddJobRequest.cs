namespace Qorpe.Scheduler.Contracts.V1.Jobs;

public record AddJobRequest(
    string Name,
    string Group,
    string? Description,
    string JobType,
    bool Durable = true,
    bool RequestsRecovery = true,
    JobDataMap? JobDataMap = null
);