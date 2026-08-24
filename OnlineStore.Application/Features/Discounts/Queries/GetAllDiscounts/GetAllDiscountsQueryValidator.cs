using FluentValidation;

namespace OnlineStore.Application.Features.Discounts.Queries.GetAllDiscounts
{
    public class GetAllDiscountsQueryValidator : AbstractValidator<GetAllDiscountsQuery>
    {

        public GetAllDiscountsQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than zero.");

            RuleFor(x => x.PageSize)
                .Must(size => size == 20 || size == 50)
                .WithMessage("Page size must be either 20 or 50.");

            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage("Invalid discount type.")
                .When(x => x.Type.HasValue);

            RuleFor(x => x.SortBy)
                .IsInEnum()
                .WithMessage("Invalid sort field.");
        }
    }
}
