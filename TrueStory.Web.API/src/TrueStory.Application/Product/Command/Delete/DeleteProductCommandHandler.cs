using MediatR;
using TrueStory.Application.Infrastructure.RestFul.ServiceImplementators;
using TrueStory.Application.Resources;
using TrueStory.Common.Exceptions;
using TrueStory.Domain.Aggregate.Product;
using TrueStory.Domain.Interfaces;

namespace TrueStory.Application.Product.Commands.Delete;

/// <summary>
/// Handles the deletion of a person.
/// </summary>
public class DeleteProductCommandHandler(IUnitOfWork _unitOfWork, IRestFulServiceImplementator _service) : IRequestHandler<DeleteProductCommand, Unit>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteProductCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work for managing transactions.</param>
    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.Id) ??
            throw new NotFoundException(string.Format(ExceptionMessageResource.NotFound, nameof(T_Product), request.Id));

        var deleteItem = await _service.DeleteProductAsync(product.ID, cancellationToken);

        _unitOfWork.ProductRepository.Delete(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}