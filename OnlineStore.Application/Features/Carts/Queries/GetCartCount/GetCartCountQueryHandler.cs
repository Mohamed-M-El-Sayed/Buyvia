using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.CartsSpecifications;
using OnlineStore.Domain.Entities.ShoppingCart;

namespace OnlineStore.Application.Features.Carts.Queries.GetCartCount
{
    public class GetCartCountQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetCartCountQueryHandler> logger,
        ICurrentUserService currentUserService) : IRequestHandler<GetCartCountQuery, int>
    {
        public async Task<int> Handle(GetCartCountQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId
                ?? throw new UnauthorizedException("User is not authenticated");
            logger.LogInformation("Getting cart count for user {UserId}", userId);
            int count = await unitOfWork.Repository<CartItem>()
                .GetCountAsync(new CartItemsCountSpecification(userId), cancellationToken);
            return count;
        }
    }
}
