namespace Qorpe.Scheduler.Contracts.V1.Jobs;

/// <summary>Strongly typed job data container for API contracts.</summary>
public sealed record JobDataMap
{
    public List<JobDataEntry<string>> Strings { get; init; } = [];
    public List<JobDataEntry<int>> Ints { get; init; } = [];
    public List<JobDataEntry<long>> Longs { get; init; } = [];
    public List<JobDataEntry<bool>> Bools { get; init; } = [];
    public List<JobDataEntry<float>> Floats { get; init; } = [];
    public List<JobDataEntry<double>> Doubles { get; init; } = [];
    public List<JobDataEntry<decimal>> Decimals { get; init; } = [];
    public List<JobDataEntry<DateTimeOffset>> Dates { get; init; } = [];
    public List<JobDataEntry<byte[]>> Bytes { get; init; } = [];
}