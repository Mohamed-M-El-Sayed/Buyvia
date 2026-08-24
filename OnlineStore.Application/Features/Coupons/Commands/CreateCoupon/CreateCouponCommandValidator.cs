using FluentValidation;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Coupons.Commands.CreateCoupon
{
    public class CreateCouponCommandValidator : AbstractValidator<CreateCouponCommand>
    {
        public CreateCouponCommandValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Coupon code is required")
                .MinimumLength(3).WithMessage("Coupon code must be at least 3 characters")
                .MaximumLength(50).WithMessage("Coupon code cannot exceed 50 characters")
                .Matches(@"^[A-Z0-9\-]*$").WithMessage("Coupon code must contain only uppercase letters, numbers, and hyphens");


            RuleFor(x => x.DiscountType)
                .IsInEnum().WithMessage("Invalid discount type");

            RuleFor(x => x.DiscountValue)
                .GreaterThan(0).WithMessage("Discount value must be greater than 0");

            RuleFor(x => x.DiscountValue)
                .LessThanOrEqualTo(100)
                .When(x => x.DiscountType == DiscountType.Percentage)
                .WithMessage("Percentage discount value must be between 1 and 100");

            RuleFor(x => x.MaxUsageCount)
                .GreaterThan(0)
                .When(x => x.MaxUsageCount.HasValue)
                .WithMessage("Maximum usage count must be greater than 0");

            RuleFor(x => x.MinOrderAmount)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MinOrderAmount.HasValue)
                .WithMessage("Minimum order amount cannot be negative");

            RuleFor(x => x.ExpiresAt)
                .GreaterThan(DateTime.UtcNow)
                .When(x => x.ExpiresAt.HasValue)
                .WithMessage("Expiration date must be in the future");

            RuleFor(x => x.ExpiresAt)
                .GreaterThan(x => x.StartsAt)
                .When(x => x.StartsAt.HasValue && x.ExpiresAt.HasValue)
                .WithMessage("Expiration date must be after start date");

            RuleFor(x => x.StartsAt)
                .GreaterThanOrEqualTo(DateTime.UtcNow)
                .When(x => x.StartsAt.HasValue)
                .WithMessage("Start date must be in the future or today");
        }
    }
}
