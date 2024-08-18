using Qorpe.Domain.Enums;

namespace Qorpe.Application.Common.DTOs;

public sealed class RouteHeaderDto
{
    public long? Id { get; set; }

    public string Name { get; set; } = default!;

    public ICollection<string>? Values { get; set; }

    public HeaderMatchMode Mode { get; set; }

    public bool IsCaseSensitive { get; set; }
}
