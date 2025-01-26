using FluentValidation;
using Twewew.Requests.OrderItem;

namespace Twewew.Validators.OrderItem;

public class UpdateOrderItemValidator : AbstractValidator<UpdateOrderItemRequest>
{
    public UpdateOrderItemValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greater than 0");

        RuleFor(x => x.Quantity)
           .GreaterThan(-1);

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage("Unit price must be greater than 0");

        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("ProductId must be greater than 0");

        RuleFor(x => x.OrderId)
            .GreaterThan(0)
            .WithMessage("OrderId must be greater than 0");
    }
}
