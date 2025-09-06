namespace Qorpe.Scheduler.Contracts.V1.Jobs;

public sealed record GroupMatcherRequest(string Operator /* Equals|StartsWith|EndsWith|Contains */, string CompareTo);