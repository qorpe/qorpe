using System.ComponentModel.DataAnnotations;

namespace Qorpe.Scheduler.Contracts.V1.Triggers;

/// <summary>Represents a Trigger identity.</summary>
public sealed record TriggerKey([Required] string Name, [Required] string Group);