using MediatR;
using TrueStory.Common.Application.Interfaces;

namespace TrueStory.Application.Product.Commands.Delete;

/// <summary>
/// Command to delete a person by their ID.
/// </summary>
/// <param name="Id">The ID of the person to be deleted.</param>
public record DeleteProductCommand(Guid Id) : IRequest<Unit>, ITransactionalRequest;