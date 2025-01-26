using FluentValidation;
using Twewew.Requests.Product;

namespace Twewew.Validators.Product;

public class CreateProductValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name must be specified")
            .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters");

        RuleFor(x => x.Description)
            .Length(5, 500).WithMessage("Product description must have between 5 and 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Product price must be specified")
            .GreaterThan(0).WithMessage("Product price must be greater than 0");

        RuleFor(x => x.AddedDate)
            .NotEmpty().WithMessage("Product addedDate must be specified");

        RuleFor(x => x.IsFrozen)
            .NotEmpty();

        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("CategoryId must be greater than 0");

    }
}
