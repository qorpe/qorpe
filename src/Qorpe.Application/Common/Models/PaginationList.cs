namespace Qorpe.Application.Common.Models;

public class PaginationList<T>(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
{
    public IReadOnlyCollection<T> Items { get; } = items;
    public int PageNumber { get; } = pageNumber;
    public int TotalPages { get; } = (int)Math.Ceiling(count / (double)pageSize);
    public int TotalCount { get; } = count;
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public static async Task<PaginationList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = 10;
        var items = new List<T>();

        return new PaginationList<T>(items, count, pageNumber, pageSize);
    }
}
