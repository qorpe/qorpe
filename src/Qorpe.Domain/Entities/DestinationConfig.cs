namespace Qorpe.Domain.Entities;

public sealed class DestinationConfig
{
    public long Id { get; set; }

    public string Address { get; set; } = default!;

    public string? Health { get; set; }

    // public IReadOnlyDictionary<string, string>? Metadata { get; set; }
    public ICollection<Metadata>? Metadata { get; set; }

    public string? Host { get; set; }

    // Foreign Key
    public long DestinationId { get; set; }
}
