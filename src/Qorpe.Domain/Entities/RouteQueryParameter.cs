using Qorpe.Domain.Enums;

namespace Qorpe.Domain.Entities;

public sealed class RouteQueryParameter
{
    public string Name { get; init; } = default!;

    public IReadOnlyList<string>? Values { get; init; }

    public QueryParameterMatchMode Mode { get; init; }

    public bool IsCaseSensitive { get; init; }
}
