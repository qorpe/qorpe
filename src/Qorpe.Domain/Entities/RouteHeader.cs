using Qorpe.Domain.Enums;

namespace Qorpe.Domain.Entities;

public sealed class RouteHeader
{
    public string Name { get; init; } = default!;

    public IReadOnlyList<string>? Values { get; init; }

    public HeaderMatchMode Mode { get; init; }

    public bool IsCaseSensitive { get; init; }
}
