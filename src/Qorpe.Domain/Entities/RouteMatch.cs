namespace Qorpe.Domain.Entities;

public sealed class RouteMatch
{
    public long Id { get; set; }

    public ICollection<string>? Methods { get; init; }

    public ICollection<string>? Hosts { get; init; }

    public string? Path { get; init; }

    public ICollection<RouteQueryParameter>? QueryParameters { get; init; }

    public ICollection<RouteHeader>? Headers { get; init; }

    // Foreign Key
    public long RouteConfigId { get; set; }
}
