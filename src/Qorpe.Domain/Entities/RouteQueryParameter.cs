﻿using Qorpe.Domain.Enums;

namespace Qorpe.Domain.Entities;

public sealed class RouteQueryParameter
{
    public string? Name { get; set; }
    public ICollection<string>? Values { get; set; }
    public QueryParameterMatchMode Mode { get; set; }
    public bool IsCaseSensitive { get; set; }
}
