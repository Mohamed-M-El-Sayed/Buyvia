using MediatR;
using OnlineStore.Application.Features.Discounts.Dtos;

namespace OnlineStore.Application.Features.Discounts.Queries.GetDiscountById
{
    public class GetDiscountByIdQuery(int discountId) : IRequest<DiscountDto>
    {
        public int DiscountId { get; } = discountId;
    }
}
