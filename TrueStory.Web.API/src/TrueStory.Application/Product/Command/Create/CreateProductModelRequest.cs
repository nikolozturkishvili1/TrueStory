using System.Text.Json;
using System.Text.Json.Serialization;

namespace TrueStory.Application.Product.Commands.Create;

/// <summary>
/// Represents the request model for creating a new product.
/// </summary>
/// <param name="Name">The name of the product.</param>
/// <param name="Data">The JSON data associated with the product.</param>
public record CreateProductModelRequest(
    string Name,
    [property: JsonPropertyName("data")] JsonElement Data
);