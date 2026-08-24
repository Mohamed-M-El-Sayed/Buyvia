using FluentValidation;

namespace OnlineStore.Application.Features.ProductVariants.Commands.UpdateVariant
{
    public class UpdateVariantCommandValidator : AbstractValidator<UpdateVariantCommand>
    {
        public UpdateVariantCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Variant Id must be greater than 0.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0m)
                .WithMessage("Price must be non-negative.");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Stock must be non-negative.");

            RuleFor(x => x.StockThreshold)
                .GreaterThanOrEqualTo(0)
                .WithMessage("StockThreshold must be non-negative.");

            RuleFor(x => x)
                .Must(x => x.Stock >= x.StockThreshold)
                .WithMessage("Stock must be greater than or equal to StockThreshold.");
        }
    }
}