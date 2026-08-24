using FluentValidation;

namespace OnlineStore.Application.Features.Orders.Commands.PlaceOrder
{
    // Features/Orders/Commands/PlaceOrder/PlaceOrderCommandValidator.cs
    public class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
    {
        public PlaceOrderCommandValidator()
        {
            RuleFor(x => x.DeliveryMethodId)
                .GreaterThan(0).WithMessage("Delivery method is required.");

            RuleFor(x => x.PaymentMethod)
                .IsInEnum().WithMessage("Invalid payment method.");
        }
    }
}
