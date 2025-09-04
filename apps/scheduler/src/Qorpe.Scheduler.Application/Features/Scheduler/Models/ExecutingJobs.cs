namespace Qorpe.Scheduler.Application.Features.Scheduler.Models;

/// <summary>
/// Wrapper response containing all currently executing jobs.
/// </summary>
public sealed record ExecutingJobs(IReadOnlyList<ExecutingJob> Mapped)
{
    public IReadOnlyList<ExecutingJob> Jobs { get; init; }
        = Mapped;
}