namespace Qorpe.Domain.Entities;

public sealed class Metadata
{
    public long Id { get; set; }

    public long ParentId { get; set; }

    public required string Key { get; set; }

    public string? Value { get; set; }
}
