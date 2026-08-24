using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Wishlists.Dtos;
using OnlineStore.Application.Features.Wishlists.Specifications;
using OnlineStore.Domain.Entities.Wishlists;

namespace OnlineStore.Application.Features.Wishlists.Queries.GetWishlist
{
    public class GetWishlistQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetWishlistQueryHandler> logger,
        IMapper mapper,
        ICurrentUserService currentUserService) : IRequestHandler<GetWishlistQuery, WishlistDto>
    {
        public async Task<WishlistDto> Handle(GetWishlistQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId
                ?? throw new UnauthorizedException("User is not authenticated");

            logger.LogInformation("Get Wishlist for user {UserId}", userId);
            var wishlist = await unitOfWork.Repository<Wishlist>()
                .GetEntityWithSpecAsync(new WishlistDetailsSpecification(userId));


            if (wishlist is null)
                return new WishlistDto();
            var wishlistDto = mapper.Map<WishlistDto>(wishlist);
            return wishlistDto;
        }
    }
}
