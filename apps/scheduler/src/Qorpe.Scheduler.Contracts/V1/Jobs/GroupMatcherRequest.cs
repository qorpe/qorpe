namespace Qorpe.Scheduler.Contracts.V1.Jobs;

public sealed record GroupMatcherRequest(GroupOp Operator, string CompareTo);