using MediatR;
using TrueStory.Application;
using TrueStory.Application.Infrastructure.RestFul.DTO;
using TrueStory.Application.Infrastructure.RestFul.ServiceImplementators;
using TrueStory.Common.Exceptions;
using TrueStory.Domain.Aggregate.Product;
using TrueStory.Domain.Interfaces;
using System.Text.Json;
using TrueStory.Common.Application.Interfaces;

namespace TrueStory.Application.Product.Commands.Create;

/// <summary>
/// Command for creating a new product.
/// </summary>
/// <param name="Name">The name of the product.</param>
/// <param name="Data">The JSON data associated with the product.</param>
public record CreateProductCommand(
    string Name,
    JsonElement Data
) : IRequest<Guid>, ITransactionalRequest;

/// <summary>
/// Handles the creation of a new product.
/// </summary>
public class CreateProductCommandHandler(IUnitOfWork _unitOfWork, IRestFulServiceImplementator _service) : IRequestHandler<CreateProductCommand, Guid>
{

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProductCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work for managing transactions.</param>
    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {

        if (await _unitOfWork.ProductRepository.AnyAsync(x => x.Name == request.Name.Trim() && !x.IsDeleted, cancellationToken))
            throw new AlreadyExistsException(string.Format(TrueStory.Application.Resources.ExceptionMessageResource.RecordAlreadyExists, request.Name));




        var newItemID = await _service.AddProductAsync(new AddProductRequestDTO 
        { 
            Name = request.Name, 
            Data = request.Data 
        }, cancellationToken);

        var product = T_Product.Create(newItemID, request.Name);

        await _unitOfWork.ProductRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return product.ID;
    }
}