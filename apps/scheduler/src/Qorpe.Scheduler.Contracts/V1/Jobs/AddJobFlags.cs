namespace Qorpe.Scheduler.Contracts.V1.Jobs;

public sealed record AddJobFlags(bool Replace, bool? StoreNonDurableWhileAwaitingScheduling);