using MediatR;
using System.Dynamic;
using System.Text.Json;
using TrueStory.Common.Application.Interfaces;

namespace PersonRegistry.Application.Product.Commands.Create;

/// <summary>
/// Command for creating a new person.
/// </summary>
/// <param name="Name">The first name of the person.</param>
/// <param name="Data">The JSON data associated with the product.</param>
public record CreateProductCommand(
    string Name,
    JsonElement Data
) : IRequest<Guid>, ITransactionalRequest;

