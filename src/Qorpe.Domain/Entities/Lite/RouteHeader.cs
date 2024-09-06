using Qorpe.Domain.Enums;

namespace Qorpe.Domain.Entities.Lite;

public sealed class RouteHeader
{
    public string Name { get; set; } = default!;
    public ICollection<string>? Values { get; set; }
    public HeaderMatchMode Mode { get; set; }
    public bool IsCaseSensitive { get; set; }
}
