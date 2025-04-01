
namespace TrueStory.Application.Product.Queries.GetProducts;

/// <summary>
/// Represents a request to retrieve a list of Products, optionally filtered by various criteria, 
/// and supports pagination.
/// </summary>
public class GetProductsModelRequest
{
    /// <summary>
    /// Filter for Products with a matching name.
    /// </summary>
    public string Name { get; set; }
   
    /// <summary>
    /// The number of items to return per page (for pagination).
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// The page number of the result set to return (for pagination).
    /// </summary>
    public int PageNumber { get; set; }
}
