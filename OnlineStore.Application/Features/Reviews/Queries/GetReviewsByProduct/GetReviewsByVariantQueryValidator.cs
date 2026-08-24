using FluentValidation;

namespace OnlineStore.Application.Features.Reviews.Queries.GetReviewsByProduct
{
    public class GetReviewsByVariantQueryValidator : AbstractValidator<GetReviewsByProductQuery>
    {
        public GetReviewsByVariantQueryValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("ProductId must be valid");

            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("PageNumber must be greater than 0");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(20, 50)
                .WithMessage("PageSize must be between 20 and 50");

            RuleFor(x => x.ExactRating)
                .InclusiveBetween(1, 5)
                .When(x => x.ExactRating.HasValue)
                .WithMessage("Rating must be between 1 and 5");

            RuleFor(x => x.SortBy)
                .IsInEnum()
                .WithMessage("Invalid sort field");
        }
    }
}
