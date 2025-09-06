using Quartz;

namespace Qorpe.Scheduler.Infrastructure.Scheduling.Jobs;

public class NoOpJob : IJob
{
    public Task Execute(IJobExecutionContext context) => Task.CompletedTask;
}