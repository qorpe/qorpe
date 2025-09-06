using System.ComponentModel.DataAnnotations;

namespace Qorpe.Scheduler.Contracts.V1.Triggers;

/// <summary>Represents a Job identity.</summary>
public sealed record JobKey([Required] string Name, [Required] string Group);