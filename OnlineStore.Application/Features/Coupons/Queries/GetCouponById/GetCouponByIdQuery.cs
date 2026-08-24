using MediatR;
using OnlineStore.Application.Features.Coupons.Dtos;

namespace OnlineStore.Application.Features.Coupons.Queries.GetCouponById
{
    public class GetCouponByIdQuery(int id) : IRequest<CouponDto>
    {
        public int Id { get; } = id;
    }
}
