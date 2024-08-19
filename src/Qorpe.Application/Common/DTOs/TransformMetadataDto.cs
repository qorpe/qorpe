namespace Qorpe.Application.Common.DTOs;

public sealed class TransformMetadataDto
{
    public long? Id { get; set; }

    public long? ParentId { get; set; }

    public string Key { get; set; }

    public string? Value { get; set; }
}
