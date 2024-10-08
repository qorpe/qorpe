﻿namespace Qorpe.Application.Common.Models;

/// <summary>
/// Represents a paginated response containing data and metadata about the pagination.
/// </summary>
/// <typeparam name="T">The type of the data items in the paginated response.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="PaginatedResponse{T}"/> class.
/// </remarks>
/// <param name="data">The collection of data items on the current page.</param>
/// <param name="totalCount">The total number of items available across all pages.</param>
/// <param name="pageNumber">The current page number.</param>
/// <param name="pageSize">The number of items per page.</param>
public class PaginatedResponse<T>(List<T> data, long totalCount, int page, int pageSize)
{
    /// <summary>
    /// Gets or sets the collection of data items on the current page.
    /// </summary>
    public List<T> Data { get; set; } = data;

    /// <summary>
    /// Gets or sets the total number of items available across all pages.
    /// </summary>
    public long TotalCount { get; set; } = totalCount;

    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    public int Page { get; set; } = page;

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    public int PageSize { get; set; } = pageSize;

    /// <summary>
    /// Gets the total number of pages based on the total count and page size.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    /// <summary>
    /// Gets a value indicating whether there is a previous page.
    /// </summary>
    public bool HasPrevious => Page > 1;

    /// <summary>
    /// Gets a value indicating whether there is a next page.
    /// </summary>
    public bool HasNext => Page < TotalPages;
}

