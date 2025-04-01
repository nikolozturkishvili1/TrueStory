using MediatR;
using TrueStory.Common.Application.Paging;

namespace TrueStory.Application.Product.Queries.GetProducts;

/// <summary>
/// Represents a query to retrieve a paginated list of Products with optional filtering.
/// </summary>
/// <remarks>
/// This query allows filtering by various fields such as name, Productal number, city, gender, and birth date. 
/// The results are paginated using the specified <paramref name="PageSize"/> and <paramref name="PageNumber"/>.
/// </remarks>
/// <param name="Name">Filters by a Product's first name.</param>

/// <param name="PageSize">The number of records to return per page.</param>
/// <param name="PageNumber">The page number to retrieve.</param>
/// <returns>A paginated list of Products that match the filter criteria.</returns>
public record GetProductsQuery(
    string Name,
    int PageSize,
    int PageNumber
) : IRequest<PagedResult<GetProductsModelResponse>>;
