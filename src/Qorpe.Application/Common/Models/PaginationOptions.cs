namespace Qorpe.Application.Common.Models;

/// <summary>
/// Represents options for pagination and sorting in queries.
/// </summary>
public class PaginationOptions
{
    /// <summary>
    /// Gets or sets the current page number. Defaults to 1.
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the number of items per page. Defaults to 10.
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets the field by which to sort the results. Defaults to "CreatedAt".
    /// </summary>
    public string SortBy { get; set; } = "CreatedAt";

    /// <summary>
    /// Gets or sets a value indicating whether the sort order is ascending. Defaults to true.
    /// </summary>
    public bool IsAscending { get; set; } = true;
}
