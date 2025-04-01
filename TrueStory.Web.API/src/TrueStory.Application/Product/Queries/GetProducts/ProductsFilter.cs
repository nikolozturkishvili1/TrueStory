
using TrueStory.Application.Product.Queries.GetProducts;
using TrueStory.Domain.Aggregate.Product;

namespace ProductRegistry.Application.Product.Queries.GetProducts;

/// <summary>
/// Provides filtering logic for querying Products based on various criteria.
/// </summary>
public static class ProductsFilter
{
    /// <summary>
    /// Applies filtering conditions to an <see cref="IQueryable{T}"/> of Products based on the provided filter criteria.
    /// </summary>
    /// <param name="query">The queryable collection of Products.</param>
    /// <param name="filter">The filtering criteria.</param>
    /// <returns>The filtered <see cref="IQueryable{T}"/>.</returns>
    public static IQueryable<T_Product> Filter(IQueryable<T_Product> query, GetProductsQuery filter)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(filter);

            query = query.Where(x => !x.IsDeleted);


        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.ToLower().Contains(filter.Name.ToLower()));

        return query;
    }
}