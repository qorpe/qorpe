namespace Qorpe.Domain.Entities;

public sealed class DestinationConfig
{
    public long Id { get; set; }

    public long DestinationId { get; set; }

    public string Address { get; init; } = default!;

    public string? Health { get; init; }

    // public IReadOnlyDictionary<string, string>? Metadata { get; init; }
    public ICollection<Metadata>? Metadata { get; init; }

    public string? Host { get; init; }
}
