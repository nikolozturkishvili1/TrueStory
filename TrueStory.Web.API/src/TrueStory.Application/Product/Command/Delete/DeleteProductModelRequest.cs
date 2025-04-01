namespace TrueStory.Application.Product.Commands.Delete;

/// <summary>
/// Represents a request to delete a person identified by their unique identifier.
/// </summary>
/// <param name="Id">The unique identifier of the person to be deleted.</param>
public record DeleteProductModelRequest(Guid Id);