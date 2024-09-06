using Qorpe.Domain.Enums;

namespace Qorpe.Application.Common.DTOs;

public sealed class RouteHeaderDto
{
    public string? Name { get; set; }
    public ICollection<string>? Values { get; set; }
    public HeaderMatchMode Mode { get; set; }
    public bool IsCaseSensitive { get; set; }
}
