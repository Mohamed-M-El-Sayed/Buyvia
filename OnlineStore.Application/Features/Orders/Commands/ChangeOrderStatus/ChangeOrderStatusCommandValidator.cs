using FluentValidation;

namespace OnlineStore.Application.Features.Orders.Commands.ChangeOrderStatus
{
    public class ChangeOrderStatusCommandValidator
         : AbstractValidator<ChangeOrderStatusCommand>
    {
        public ChangeOrderStatusCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0)
                .WithMessage("Order ID must be greater than 0.");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid order status.");
        }
    }
}
