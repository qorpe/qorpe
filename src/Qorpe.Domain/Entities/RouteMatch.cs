namespace Qorpe.Domain.Entities;

public sealed class RouteMatch
{
    public IReadOnlyList<string>? Methods { get; init; }

    public IReadOnlyList<string>? Hosts { get; init; }

    public string? Path { get; init; }

    public IReadOnlyList<RouteQueryParameter>? QueryParameters { get; init; }

    public IReadOnlyList<RouteHeader>? Headers { get; init; }
}
