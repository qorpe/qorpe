namespace Qorpe.Application.Common.DTOs;

public sealed class DestinationConfigDto
{
    public string? Address { get; set; }
    public string? Health { get; set; }
    public Dictionary<string, string>? Metadata { get; set; }
    public string? Host { get; set; }
}
