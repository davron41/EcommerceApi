using FluentValidation;
using Twewew.Requests.Order;

namespace Twewew.Validators.Order;

public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.OrderDate)
            .NotEmpty()
            .WithMessage("OrderDate must be specified");

        RuleFor(x => x.TotalQuantity)
            .GreaterThan(-1);

        RuleFor(x => x.OrderStatus)
            .IsInEnum().WithMessage("Invalid order status provided.");
    }
}
