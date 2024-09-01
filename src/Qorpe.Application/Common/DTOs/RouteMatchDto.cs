namespace Qorpe.Application.Common.DTOs;

public sealed class RouteMatchDto
{
    public ICollection<string>? Methods { get; set; }
    public ICollection<string>? Hosts { get; set; }
    public string? Path { get; set; }
    public ICollection<RouteQueryParameterDto>? QueryParameters { get; set; }
    public ICollection<RouteHeaderDto>? Headers { get; set; }
}
