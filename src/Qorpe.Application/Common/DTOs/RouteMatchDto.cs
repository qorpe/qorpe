namespace Qorpe.Application.Common.DTOs;

public sealed class RouteMatchDto
{
    public long? Id { get; set; }

    public ICollection<string>? Methods { get; set; }

    public ICollection<string>? Hosts { get; set; }

    public string? Path { get; set; }

    public ICollection<RouteQueryParameterDto>? QueryParameters { get; set; }

    public ICollection<RouteHeaderDto>? Headers { get; set; }

    // Foreign Key
    public long? RouteConfigId { get; set; }
}
