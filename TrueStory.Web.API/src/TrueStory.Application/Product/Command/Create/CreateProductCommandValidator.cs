using FluentValidation;
using TrueStory.Application.Product.Helper;
using TrueStory.Application.Resources;

namespace TrueStory.Application.Product.Commands.Create;

/// <summary>
/// Validator for the <see cref="CreateProductCommand"/>.
/// </summary>
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProductCommandValidator"/> class.
    /// </summary>
    public CreateProductCommandValidator()
    {

        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(2, 50)
            .Must(ValidationHelper.ValidateForMixedLanguages)
            .WithMessage(x => string.Format(ValidationResources.MixedLanguage, nameof(x.Name)));

        RuleFor(x => x.Data)
            .Must(ValidationHelper.ValidateJsonFormat)
            .WithMessage(x => string.Format(ValidationResources.NotJson, nameof(x.Data)));
    }
}