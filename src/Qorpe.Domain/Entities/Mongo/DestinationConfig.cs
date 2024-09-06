namespace Qorpe.Domain.Entities.Mongo;

public sealed class DestinationConfig
{
    public string Address { get; set; } = default!;
    public string? Health { get; set; }
    public Dictionary<string, string>? Metadata { get; set; }
    public string? Host { get; set; }
}
