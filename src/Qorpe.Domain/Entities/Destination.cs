namespace Qorpe.Domain.Entities;

public sealed class Destination
{
    public long Id { get; set; }
    public long ParentId { get; set; }
    public required string Key { get; set; }
    public DestinationConfig? Value { get; set; }
}
