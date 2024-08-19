namespace Qorpe.Application.Common.DTOs;

public sealed class DestinationConfigDto
{
    public long? Id { get; set; }

    public string Address { get; set; } = default!;

    public string? Health { get; set; }

    // public IReadOnlyDictionary<string, string>? Metadata { get; set; }
    public ICollection<DestinationConfigMetadataDto>? Metadata { get; set; }

    public string? Host { get; set; }

    // Foreign Key
    public long? DestinationId { get; set; }
}
