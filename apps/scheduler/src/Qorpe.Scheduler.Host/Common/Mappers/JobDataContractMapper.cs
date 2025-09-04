using Quartz;
using Jobs = Qorpe.Scheduler.Contracts.V1.Jobs;

namespace Qorpe.Scheduler.Host.Common.Mappers;

/// <summary>Maps typed JobDataMapDto to/from Quartz.JobDataMap.</summary>
public static class JobDataContractMapper
{
    /// <summary>Creates JobDataMap from typed dto.</summary>
    public static JobDataMap ToJobDataMap(Jobs.JobDataMap? dto)
    {
        var map = new JobDataMap();
        if (dto is null) return map;

        // Strings
        foreach (var e in dto.Strings) map[e.Key] = e.Value;
        // Numbers/Booleans
        foreach (var e in dto.Ints) map[e.Key] = e.Value;
        foreach (var e in dto.Longs) map[e.Key] = e.Value;
        foreach (var e in dto.Bools) map[e.Key] = e.Value;
        foreach (var e in dto.Floats) map[e.Key] = e.Value;
        foreach (var e in dto.Doubles) map[e.Key] = e.Value;
        foreach (var e in dto.Decimals) map[e.Key] = e.Value;
        // Dates (store as DateTimeOffset to preserve tz)
        foreach (var e in dto.Dates) map[e.Key] = e.Value;
        // Binary
        foreach (var e in dto.Bytes) map[e.Key] = e.Value;

        return map;
    }

    /// <summary>Builds typed dto from JobDataMap (best-effort typing).</summary>
    public static Jobs.JobDataMap FromJobDataMap(JobDataMap map)
    {
        var dto = new Jobs.JobDataMap();
        foreach (var kv in map)
        {
            var (key, val) = (kv.Key, kv.Value);
            switch (val)
            {
                case null: dto.Strings.Add(new(key, string.Empty)); break;
                case string s: dto.Strings.Add(new(key, s)); break;
                case int i: dto.Ints.Add(new(key, i)); break;
                case long l: dto.Longs.Add(new(key, l)); break;
                case bool b: dto.Bools.Add(new(key, b)); break;
                case float f: dto.Floats.Add(new(key, f)); break;
                case double d: dto.Doubles.Add(new(key, d)); break;
                case decimal m: dto.Decimals.Add(new(key, m)); break;
                case DateTimeOffset dtoff: dto.Dates.Add(new(key, dtoff)); break;
                case DateTime dt: dto.Dates.Add(new(key, new DateTimeOffset(dt))); break;
                case byte[] bytes: dto.Bytes.Add(new(key, bytes)); break;
                default:
                    // Fallback: serialize unknowns to string
                    dto.Strings.Add(new(key, val.ToString() ?? string.Empty));
                    break;
            }
        }
        return dto;
    }
}