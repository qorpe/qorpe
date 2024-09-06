namespace Qorpe.Domain.Entities;

public sealed class RouteMatch
{
    public ICollection<string>? Methods { get; set; }
    public ICollection<string>? Hosts { get; set; }
    public string? Path { get; set; }
    public ICollection<RouteQueryParameter>? QueryParameters { get; set; }
    public ICollection<RouteHeader>? Headers { get; set; }
}
