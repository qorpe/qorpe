using Qorpe.Domain.Enums;

namespace Qorpe.Domain.Entities;

public sealed class RouteQueryParameter
{
    public long? Id { get; set; }

    public string Name { get; set; } = default!;

    public ICollection<string>? Values { get; set; }

    public QueryParameterMatchMode Mode { get; set; }

    public bool IsCaseSensitive { get; set; }

    // Foreign Key
    public long? RouteMatchId { get; set; }
}
