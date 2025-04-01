using FluentValidation;

namespace TrueStory.Application.Product.Commands.Delete;

/// <summary>
/// Validator for <see cref="DeleteProductCommand"/>.
/// Ensures the ID is provided and valid.
/// </summary>
public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteProductCommandValidator"/> class.
    /// </summary>
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}