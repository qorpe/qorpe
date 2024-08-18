using Qorpe.Domain.Enums;

namespace Qorpe.Domain.Entities;

public sealed class RouteQueryParameter
{
    public long Id { get; set; }

    public string Name { get; init; } = default!;

    public ICollection<string>? Values { get; init; }

    public QueryParameterMatchMode Mode { get; init; }

    public bool IsCaseSensitive { get; init; }

    // Foreign Key
    public long RouteMatchId { get; set; }
}
