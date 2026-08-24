using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Carts.Dtos;
using OnlineStore.Application.Features.Carts.Specifications;
using OnlineStore.Domain.Entities.ShoppingCart;

namespace OnlineStore.Application.Features.Carts.Queries.GetCart
{
    public class GetCartQueryHandler(
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            ILogger<GetCartQueryHandler> logger,
            IMapper mapper
        ) : IRequestHandler<GetCartQuery, CartDto>
    {
        public async Task<CartDto> Handle(
            GetCartQuery request,
            CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId
                ?? throw new UnauthorizedException("User is not authenticated");

            logger.LogInformation("Getting cart for user {UserId}", userId);

            var cart = await unitOfWork.Repository<Cart>()
                .GetEntityWithSpecAsync(
                    new CartDetailsSpecification(userId),
                    cancellationToken);

            if (cart is null)
            {
                return new CartDto
                {
                    Items = new()
                };
            }

            var dto = mapper.Map<CartDto>(cart);

            if (cart.Coupon is not null)
            {
                var amount = dto.SubTotal - dto.ItemsDiscount;

                if (cart.Coupon.IsValid(amount))
                {
                    dto.CouponDiscount =
                        cart.Coupon.CalculateDiscount(amount);
                }
                else
                {
                    dto.CouponCode = null;
                    dto.CouponDiscount = 0;
                }
            }

            return dto;
        }
    }
}
