using FluentValidation;

namespace OnlineStore.Application.Features.ProductVariants.Commands.AddVariant
{
    public class CreateVariantCommandValidator : AbstractValidator<CreateVariantCommand>
    {
        public CreateVariantCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("ProductId must be greater than 0");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Stock must be greater than or equal to 0");
            RuleFor(x => x.StockThreshold)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Stock Threshold must be greater than or equal to 0")
                .LessThanOrEqualTo(x => x.Stock)
                .WithMessage("Stock Threshold cannot be greater than Stock");

            //RuleFor(x => x.MainImageUrl)
            //    .NotEmpty()
            //    .WithMessage("MainImage is required");
        }
    }
}
