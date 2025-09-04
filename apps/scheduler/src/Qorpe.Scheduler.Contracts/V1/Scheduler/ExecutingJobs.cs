namespace Qorpe.Scheduler.Contracts.V1.Scheduler;

/// <summary>
/// Wrapper response containing all currently executing jobs.
/// </summary>
public sealed record ExecutingJobs(IReadOnlyList<ExecutingJob> Mapped)
{
    public IReadOnlyList<ExecutingJob> Jobs { get; init; }
        = Mapped;
}