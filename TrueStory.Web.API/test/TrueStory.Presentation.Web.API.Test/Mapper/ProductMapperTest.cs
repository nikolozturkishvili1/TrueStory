



using TrueStory.Application.Product.Commands.Create;
using TrueStory.Application.Product.Commands.Delete;
using TrueStory.Application.Product.Queries.GetProducts;

namespace ProductRegistry.API.Mappers;

/// <summary>
/// Provides static mapping methods for converting request models to commands and queries.
/// </summary>
public static class ProductMapperTest
{


    /// <summary>
    /// Maps a <see cref="GetProductsModelRequest"/> to a <see cref="GetProductsQuery"/>.
    /// </summary>
    public static GetProductsQuery ToGetProductQuery(this GetProductsModelRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new GetProductsQuery(
            request.Name, 
            request.PageSize,
            request.PageNumber);
    }

    /// <summary>
    /// Maps a <see cref="CreateProductModelRequest"/> to a <see cref="CreateProductCommand"/>.
    /// </summary>
    public static CreateProductCommand ToCreateProductCommand(this CreateProductModelRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new CreateProductCommand(
            request.Name,
            request.Data);
    }

  

    /// <summary>
    /// Maps a <see cref="DeleteProductModelRequest"/> to a <see cref="DeleteProductCommand"/>.
    /// </summary>
    public static DeleteProductCommand ToDeleteProductCommand(this DeleteProductModelRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new DeleteProductCommand(request.Id);
    }
}
