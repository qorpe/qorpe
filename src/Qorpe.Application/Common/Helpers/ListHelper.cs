using System.Linq.Expressions;

namespace Qorpe.Application.Common.Helpers;

/// <summary>
/// A helper class that provides functionality for filtering, sorting, and pagination
/// on in-memory lists of data. This class is designed to handle large datasets
/// efficiently with the ability to dynamically apply filters, sort orders, and paginate results.
/// </summary>
public static class ListHelper
{
    /// <summary>
    /// Applies filtering, sorting, and pagination on an in-memory list of data.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source list.</typeparam>
    /// <param name="source">The source list to process.</param>
    /// <param name="filter">A filter function to apply to the list items.</param>
    /// <param name="sortBy">The property name to sort the list by (optional).</param>
    /// <param name="IsAscending">Indicates whether the sorting should be in ascending order (default is false).</param>
    /// <param name="pageNumber">The page number to retrieve (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A filtered, sorted, and paginated list of items.</returns>
    public static List<T> ApplyFilteringSortingAndPagination<T>(
        List<T> source,
        Func<T, bool> filter,
        string? sortBy = null,
        bool IsAscending = false,
        int pageNumber = 1,
        int pageSize = 10)
    {
        // 1. Filtering
        // Applies the filter function to the source list.
        var query = source.Where(filter);

        // 2. Sorting
        // Applies sorting to the filtered list if a sortBy property is provided.
        if (!string.IsNullOrEmpty(sortBy))
        {
            query = ApplySorting(query, sortBy, IsAscending);
        }

        // 3. Pagination
        // Applies pagination to the sorted and filtered list.
        // Skips the number of items based on the current page number and page size, then takes the specified number of items.
        return query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
    }

    /// <summary>
    /// Applies dynamic sorting to the provided data source based on a specified property.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source list.</typeparam>
    /// <param name="source">The source list to sort.</param>
    /// <param name="sortBy">The property name to sort the list by.</param>
    /// <param name="IsAscending">Indicates whether the sorting should be in ascending order.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> sorted by the specified property.</returns>
    private static IEnumerable<T> ApplySorting<T>(IEnumerable<T> source, string sortBy, bool IsAscending)
    {
        // Create a parameter expression representing an item in the list (e.g., x => x.PropertyName)
        var param = Expression.Parameter(typeof(T), "x");

        // Access the property specified by sortBy on the item
        var property = Expression.Property(param, sortBy);

        // Create a lambda expression to convert the property to an object (x => (object)x.PropertyName)
        var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), param);

        // Apply sorting based on the lambda expression
        // If IsAscending is true, apply OrderBy; otherwise, apply OrderByDescending
        return IsAscending
            ? source.AsQueryable().OrderBy(lambda)
            : source.AsQueryable().OrderByDescending(lambda);
    }
}