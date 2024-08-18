namespace Qorpe.Application.Common.DTOs;

public sealed class DestinationDto
{
    public long? Id { get; set; }
    public long? ParentId { get; set; }
    public string Key { get; set; }
    public DestinationConfigDto? Value { get; set; }
}
