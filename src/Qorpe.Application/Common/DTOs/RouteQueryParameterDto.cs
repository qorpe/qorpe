using Qorpe.Domain.Enums;

namespace Qorpe.Application.Common.DTOs;

public sealed class RouteQueryParameterDto
{
    public string? Name { get; set; }
    public ICollection<string>? Values { get; set; }
    public QueryParameterMatchMode Mode { get; set; }
    public bool IsCaseSensitive { get; set; }
}
