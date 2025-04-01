using TrueStory.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using TrueStory.Domain.Base;
namespace TrueStory.Common.Application.Paging;

/// <summary>
/// Provides base functionality for paginated queries.
/// </summary>
public abstract class BasePaging
{
    private int _pageNumber = 1;
    private int _pageSize = 10;

    /// <summary>
    /// Gets or sets the page number. Defaults to 1 if set to a non-positive value.
    /// </summary>
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = (value <= 0) ? _pageNumber : value;
    }

    /// <summary>
    /// Gets or sets the page size (number of items per page). Defaults to 10 if set to a non-positive value.
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value <= 0) ? _pageSize : value;
    }

    /// <summary>
    /// Calculates the number of records to skip based on the current page number and page size.
    /// </summary>
    public int SkipCount => (PageNumber - _pageNumber) * PageSize;

    /// <summary>
    /// Creates a paged result set from a queryable data source.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <typeparam name="TResponse">The DTO type for the response.</typeparam>
    /// <param name="repository">The repository instance to access the data.</param>
    /// <param name="query">The queryable source of entities.</param>
    /// <param name="pageNumber">The page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A paged result containing the data and pagination details.</returns>
    /// <exception cref="ArgumentException">Thrown if pageNumber or pageSize is invalid.</exception>
    public static async Task<PagedResult<TResponse>> CreatePagedItemsAsync<T,TKey,TResponse>(
        IBaseRepository<T, TKey> repository,
        IQueryable<T> query,
        int pageNumber,
        int pageSize)
        where T : Entity<TKey>
    {
        pageNumber = pageNumber > 0 ? pageNumber : 1;
        pageSize = pageSize > 0 ? pageSize : 10;

        var totalItemCount = await repository.CountAsync(query);

        var fetchedItems = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var items = fetchedItems
            .Select(selector: EntityMapper<T, TResponse>.Map)
            .ToList();

        return new PagedResult<TResponse>
        {
            Items = items,
            TotalItemCount = totalItemCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalItemCount / (double)pageSize)
        };
    }
}
