﻿namespace Qorpe.Application.Common.Models;

public class PaginationParameters
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? SortBy { get; set; }

    public string SortOrder { get; set; } = "asc";
}