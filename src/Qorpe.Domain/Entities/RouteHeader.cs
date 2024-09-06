using Qorpe.Domain.Enums;

namespace Qorpe.Domain.Entities;

public sealed class RouteHeader
{
    public string? Name { get; set; }
    public ICollection<string>? Values { get; set; }
    public HeaderMatchMode Mode { get; set; }
    public bool IsCaseSensitive { get; set; }
}
