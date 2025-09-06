namespace Qorpe.Scheduler.Contracts.V1.Jobs;

/// <summary>Typed entry item.</summary>
public record JobDataEntry<T>(string Key, T Value);