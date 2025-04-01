
namespace TrueStory.Application.Product.Queries.GetProducts;

/// <summary>
/// Represents a response model containing basic Product details.
/// </summary>
/// <param name="Name">The first name of the Product.</param>
///

public record GetProductsModelResponse(
    Guid ID,
    string Name
)
{
    public object? Data { get; init; }
};
