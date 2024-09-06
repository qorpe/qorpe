namespace Qorpe.Domain.Entities;

public sealed class ActiveHealthCheckConfig
{
    public bool? Enabled { get; set; }
    public TimeSpan? Interval { get; set; }
    public TimeSpan? Timeout { get; set; }
    public string? Policy { get; set; }
    public string? Path { get; set; }
    public string? Query { get; set; }
}
