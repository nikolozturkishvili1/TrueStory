namespace TrueStory.Common.Application.Paging;

/// <summary>
/// Represents a paginated result set.
/// </summary>
/// <typeparam name="T">The type of items in the paginated result.</typeparam>
public class PagedResult<T>
{
    /// <summary>
    /// Gets or sets the collection of items in the current page.
    /// </summary>
    public List<T> Items { get; set; } = new();

    /// <summary>
    /// Gets or sets the total number of items across all pages.
    /// </summary>
    public int TotalItemCount { get; set; }

    /// <summary>
    /// Gets or sets the current page number (1-based index).
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages based on the total items and page size.
    /// </summary>
    public int TotalPages { get; set; }
}