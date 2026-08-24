using FluentValidation;

namespace OnlineStore.Application.Features.Orders.Queries.GetRefundRequests
{
    public class GetRefundRequestsQueryValidator
        : AbstractValidator<GetRefundRequestsQuery>
    {
        public GetRefundRequestsQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page index must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(20, 50)
                .WithMessage("Page size must be between 20 and 50.");

            RuleFor(x => x.Status)
                .IsInEnum()
                .When(x => x.Status.HasValue)
                .WithMessage("Invalid refund request status.");
        }
    }
}