using Microsoft.Extensions.Logging;
using Quartz;

namespace Qorpe.Scheduler.Infrastructure.Scheduling.Jobs;

/// <summary>
/// Emits a heartbeat log periodically.
/// </summary>
[DisallowConcurrentExecution] // Optional: prevent overlapping runs
public sealed class HeartbeatJob(ILogger<HeartbeatJob> logger) : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        var fired = DateTimeOffset.UtcNow;
        logger.LogInformation("HeartbeatJob fired at {Time} (FireInstanceId={Id})",
            fired, context.FireInstanceId);
        return Task.CompletedTask;
    }
}