﻿using FluentValidation;
using Twewew.Requests.Category;

namespace Twewew.Validators.Category;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Invalid category ID.It must be greater than 0");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name must be specified")
            .Length(5, 255).WithMessage("Category name must have between 5 and 255 characters");

        RuleFor(x => x.Description)
            .Length(5, 500).WithMessage("Category description must have between 5 and 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}
