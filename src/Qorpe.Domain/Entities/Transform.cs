namespace Qorpe.Domain.Entities;

public sealed class Transform
{
    public long? Id { get; set; }

    public ICollection<Metadata>? Metadata { get; set; }

    // Foreign Key
    public long? RouteConfigId { get; set; }
}
