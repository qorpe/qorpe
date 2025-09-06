namespace Qorpe.Scheduler.Contracts.V1.Jobs;

public sealed record JobExecutionContext(
    string FireInstanceId,
    DateTimeOffset FireTimeUtc,
    JobKey JobDetailKey,
    string? JobDetailDescription,
    Trigger Trigger
);