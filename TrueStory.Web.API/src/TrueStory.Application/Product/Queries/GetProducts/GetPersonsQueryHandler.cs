using MediatR;
using ProductRegistry.Application.Product.Queries.GetProducts;
using TrueStory.Application.Common.Mapper;
using TrueStory.Application.Infrastructure.RestFul.ServiceImplementators;
using TrueStory.Common.Application.Paging;
using TrueStory.Domain.Aggregate.Product;
using TrueStory.Domain.Interfaces;

namespace TrueStory.Application.Product.Queries.GetProducts;

/// <summary>
/// Handles the retrieval of a paginated list of Products based on filter criteria.
/// </summary>
public class GetProductsQueryHandler(IUnitOfWork _unitOfWork, IRestFulServiceImplementator _service) : IRequestHandler<GetProductsQuery, PagedResult<GetProductsModelResponse>>
{
    /// <summary>
    /// Handles the query request to retrieve paginated Products based on filters.
    /// </summary>
    /// <param name="request">The query request containing filter parameters.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A paginated result set of Products matching the filter criteria.</returns>
    public async Task<PagedResult<GetProductsModelResponse>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var queryFiltered = ProductsFilter.Filter(_unitOfWork.ProductRepository.Query(),request);

        var pages = await BasePaging.CreatePagedItemsAsync<T_Product, Guid, GetProductsModelResponse>
                           (_unitOfWork.ProductRepository, queryFiltered, request.PageNumber, request.PageSize);

        if (pages.Items.Count == 0)
            return pages;
        

        var response = await _service.GetProductsAsync(pages.Items.Select(x => x.ID).ToList(), cancellationToken);

        pages.Items = ProductMapper.MapToProductsResponse(pages.Items,response);

        return pages;
    }
}