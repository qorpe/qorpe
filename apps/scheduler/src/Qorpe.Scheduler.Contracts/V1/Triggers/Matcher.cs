using System.ComponentModel.DataAnnotations;

namespace Qorpe.Scheduler.Contracts.V1.Triggers;

/// <summary>Represents a group matcher for TriggerKey.</summary>
public sealed record Matcher([Required] MatcherOp Op, [Required] string Expr);