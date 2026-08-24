using FluentValidation;

namespace OnlineStore.Application.Features.Carts.Commands.AddItem
{
    public class AddToCartCommandValidator : AbstractValidator<AddToCartCommand>
    {

        public AddToCartCommandValidator()
        {

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Quantity cannot exceed 100 items.");


        }


    }
}