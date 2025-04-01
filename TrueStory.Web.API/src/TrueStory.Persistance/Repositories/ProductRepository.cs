using PersonRegistry.Persistence.Repositories.Base;
using TrueStory.Domain.Aggregate.Product;
using TrueStory.Domain.Interfaces;
using TrueStory.Persistence.Context;

namespace PersonRegistry.Persistence.Repositories;

/// <summary>
/// Repository for Product
/// </summary>
internal class ProductRepository : BaseRepository<T_Product,Guid>, IProductRepository
{
    public ProductRepository(ProductDbContext context)
        : base(context)
    {
    }
}
