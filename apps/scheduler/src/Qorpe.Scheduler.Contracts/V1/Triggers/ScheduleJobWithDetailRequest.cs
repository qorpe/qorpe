using System.ComponentModel.DataAnnotations;

namespace Qorpe.Scheduler.Contracts.V1.Triggers;

/// <summary>Job detail creation + trigger schedule.</summary>
public sealed record ScheduleJobWithDetailRequest(
    // Job detail definition (type is an assembly-qualified name)
    [Required] string JobName,
    [Required] string JobGroup,
    [Required] string JobType,
    Dictionary<string, object?>? Data,
    Trigger Trigger,
    bool Durable = true,
    bool RequestsRecovery = false
);