namespace Qorpe.Scheduler.Contracts.V1.Jobs;

public sealed record GroupMatcherRequest(string Mode /* Equals|StartsWith|EndsWith|Contains */, string Pattern);