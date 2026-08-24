using FluentValidation;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Discounts.Commands.UpdateDiscount
{
    public class UpdateDiscountCommandValidator : AbstractValidator<UpdateDiscountCommand>
    {
        public UpdateDiscountCommandValidator()
        {
            RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("DiscountId is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Discount name is required.")
                .MaximumLength(100).WithMessage("Discount name cannot exceed 100 characters.");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid discount type.");

            RuleFor(x => x.Value)
                .GreaterThan(0).WithMessage("Discount value must be greater than zero.");

            RuleFor(x => x.Value)
                .LessThanOrEqualTo(100)
                .WithMessage("Percentage discount cannot exceed 100%.")
                .When(x => x.Type == DiscountType.Percentage);

            RuleFor(x => x.MaxDiscountAmount)
                .GreaterThan(0).WithMessage("Max discount amount must be greater than zero.")
                .When(x => x.MaxDiscountAmount.HasValue);

            RuleFor(x => x.MaxDiscountAmount)
                .Null().WithMessage("Max discount amount is only valid for percentage discounts.")
                .When(x => x.Type == DiscountType.FixedAmount && x.MaxDiscountAmount.HasValue);

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