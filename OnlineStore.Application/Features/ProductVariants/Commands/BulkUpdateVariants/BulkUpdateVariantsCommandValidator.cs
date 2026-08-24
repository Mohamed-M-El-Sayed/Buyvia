using FluentValidation;

namespace OnlineStore.Application.Features.ProductVariants.Commands.BulkUpdateVariants
{
    public class BulkUpdateVariantsCommandValidator : AbstractValidator<BulkUpdateVariantsCommand>
    {
        public BulkUpdateVariantsCommandValidator()
        {


            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ProductId is required.");

            RuleFor(x => x.Variants)
                .NotEmpty().WithMessage("At least one variant is required.");

            RuleForEach(x => x.Variants).ChildRules(variant =>
            {
                variant.RuleFor(v => v.Id)
                    .GreaterThan(0).WithMessage("Variant Id is required.");

                variant.RuleFor(v => v.Price)
                    .GreaterThan(0).WithMessage("Price must be greater than zero.");

                variant.RuleFor(v => v.Stock)
                    .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.");

                variant.RuleFor(v => v.StockThreshold)
                    .GreaterThanOrEqualTo(0).WithMessage("Stock threshold cannot be negative.")
                    .LessThan(v => v.Stock).WithMessage("Stock threshold must be less than stock.");
            });
        }
    }
}
