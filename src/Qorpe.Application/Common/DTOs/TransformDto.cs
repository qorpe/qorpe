namespace Qorpe.Application.Common.DTOs;

public sealed class TransformDto
{
    public long? Id { get; set; }

    public ICollection<TransformMetadataDto>? Metadata { get; set; }

    // Foreign Key
    public long? RouteConfigId { get; set; }
}
