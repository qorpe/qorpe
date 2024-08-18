using Qorpe.Domain.Enums;

namespace Qorpe.Domain.Entities;

public sealed class RouteHeader
{
    public long? Id { get; set; }

    public string Name { get; set; } = default!;

    public ICollection<string>? Values { get; set; }

    public HeaderMatchMode Mode { get; set; }

    public bool IsCaseSensitive { get; set; }
}
