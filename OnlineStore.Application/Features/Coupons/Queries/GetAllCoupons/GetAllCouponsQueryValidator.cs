using FluentValidation;

namespace OnlineStore.Application.Features.Coupons.Queries.GetAllCoupons;

public class GetAllCouponsQueryValidator
    : AbstractValidator<GetAllCouponsQuery>
{
    public GetAllCouponsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(20, 60)
            .WithMessage("Page size must be between 20 and 60.");
    }
}