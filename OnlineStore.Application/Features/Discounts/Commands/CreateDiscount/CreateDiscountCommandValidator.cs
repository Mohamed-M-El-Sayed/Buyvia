using FluentValidation;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Discounts.Commands.CreateDiscount
{

    public class CreateDiscountCommandValidator : AbstractValidator<CreateDiscountCommand>
    {
        public CreateDiscountCommandValidator()
        {
            RuleFor(x => x.Name)
                   .NotEmpty().WithMessage("Discount name is required.")
                    .MaximumLength(100).WithMessage("Discount name cannot exceed 100 characters.");


            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid discount type.");

            RuleFor(x => x.Value)
                .GreaterThan(0);

            RuleFor(x => x.Value)
                .LessThanOrEqualTo(100)
                .When(x => x.Type == DiscountType.Percentage)
                .WithMessage("Percentage value must be between 1 and 100");

            RuleFor(x => x.MaxDiscountAmount)
                .GreaterThan(0)
                .When(x => x.MaxDiscountAmount.HasValue);

            // MaxDiscountAmount only makes sense for Percentage
            RuleFor(x => x.MaxDiscountAmount)
                .Null()
                .When(x => x.Type != DiscountType.Percentage)
                .WithMessage("MaxDiscountAmount is only valid for Percentage discounts");

            RuleFor(x => x.StartsAt)
             .GreaterThanOrEqualTo(DateTime.UtcNow)
             .WithMessage("Start date cannot be in the past.")
             .When(x => x.StartsAt.HasValue);

            RuleFor(x => x.ExpiresAt)
                .GreaterThan(x => x.StartsAt ?? DateTime.UtcNow)
                .WithMessage("Expiry date must be after start date.")
                .When(x => x.ExpiresAt.HasValue);




        }
    }
}
