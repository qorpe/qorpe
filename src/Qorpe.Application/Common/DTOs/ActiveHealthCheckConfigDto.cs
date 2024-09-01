namespace Qorpe.Application.Common.DTOs;

public sealed class ActiveHealthCheckConfigDto
{
    public bool? Enabled { get; set; }
    public TimeSpan? Interval { get; set; }
    public TimeSpan? Timeout { get; set; }
    public string? Policy { get; set; }
    public string? Path { get; set; }
    public string? Query { get; set; }
}
