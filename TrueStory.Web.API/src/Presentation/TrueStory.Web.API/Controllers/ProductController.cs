using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrueStory.Application.Product.Commands.Create;
using TrueStory.Application.Product.Commands.Delete;
using ProductRegistry.API.Mappers;
using TrueStory.Application.Product.Queries.GetProducts;
using TrueStory.Common.Application.Paging;

namespace TrueStory.Web.API.Controllers
{
    [Route("api/[controller]")]
    public class ProductController(IMediator _mediator) : ControllerBase
    {


        /// <summary>
        /// Get all Products with filter
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet(nameof(GetProducts))]
        public async Task<PagedResult<GetProductsModelResponse>> GetProducts([FromQuery] GetProductsModelRequest request, CancellationToken cancellationToken)
            => await _mediator.Send(request.ToGetProductQuery(), cancellationToken);





        /// <summary>
        /// Creates new Product
        /// </summary>
        /// <param name="request.name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost(Name = nameof(PostProduct))]
        public async Task<Guid> PostProduct([FromBody] CreateProductModelRequest request, CancellationToken cancellationToken) =>
            await _mediator.Send(request.ToCreateProductCommand(), cancellationToken);


        /// <summary>
        /// Deletes Product by Id
        /// </summary>
        /// <param name="request.Id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete(nameof(DeleteProduct))]
        public async Task DeleteProduct([FromBody] DeleteProductModelRequest request, CancellationToken cancellationToken) =>
            await _mediator.Send(request.ToDeleteProductCommand(), cancellationToken);



    }
}
