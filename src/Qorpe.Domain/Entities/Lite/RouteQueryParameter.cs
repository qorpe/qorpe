using Qorpe.Domain.Enums;

namespace Qorpe.Domain.Entities.Lite;

public sealed class RouteQueryParameter
{
    public string Name { get; set; } = default!;
    public ICollection<string>? Values { get; set; }
    public QueryParameterMatchMode Mode { get; set; }
    public bool IsCaseSensitive { get; set; }
}
