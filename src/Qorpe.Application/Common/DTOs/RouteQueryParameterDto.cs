using Qorpe.Domain.Enums;

namespace Qorpe.Application.Common.DTOs;

public sealed class RouteQueryParameterDto
{
    public long? Id { get; set; }

    public string Name { get; set; } = default!;

    public ICollection<string>? Values { get; set; }

    public QueryParameterMatchMode Mode { get; set; }

    public bool IsCaseSensitive { get; set; }

    // Foreign Key
    public long? RouteMatchId { get; set; }
}
