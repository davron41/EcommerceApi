using FluentValidation;
using Twewew.Requests.Order;

namespace Twewew.Validators.Order;

public class UpdateOrderValidator : AbstractValidator<UpdateOrderRequest>
{
    public UpdateOrderValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Order id must be grater than 0");

        RuleFor(x => x.OrderDate)
          .NotEmpty()
          .WithMessage("OrderDate must be specified");

        RuleFor(x => x.TotalQuantity)
            .GreaterThan(-1);

        RuleFor(x => x.OrderStatus)
            .IsInEnum().WithMessage("Invalid order status provided.");
    }
}
