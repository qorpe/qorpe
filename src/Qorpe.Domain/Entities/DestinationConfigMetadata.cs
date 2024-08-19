namespace Qorpe.Domain.Entities;

public sealed class DestinationConfigMetadata
{
    public long? Id { get; set; }

    public long? ParentId { get; set; }

    public string Key { get; set; }

    public string? Value { get; set; }
}
