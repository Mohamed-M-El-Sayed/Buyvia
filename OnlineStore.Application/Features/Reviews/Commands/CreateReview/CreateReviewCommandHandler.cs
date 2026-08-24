using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities;
using OnlineStore.Domain.Entities.Orders;
using OnlineStore.Domain.Entities.Products;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewCommandHandler(IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CreateReviewCommandHandler> logger,
        ICurrentUserService currentUserService) : IRequestHandler<CreateReviewCommand, int>
    {
        public async Task<int> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId
                ?? throw new UnauthorizedException("User must be authenticated.");
            
            logger.LogInformation(
             "User {UserId} creating review for variant {VariantId}",
             userId, request.PurchasedVariantId);


            var variant = await unitOfWork.Repository<ProductVariant>()
                   .GetByIdAsync(request.PurchasedVariantId)
                   ?? throw new NotFoundException(
                       nameof(ProductVariant), request.PurchasedVariantId.ToString());
            var hasPurchased = await unitOfWork.Repository<OrderItem>()
                .AnyAsync(
                    oi =>
                        oi.ProductVariantId == request.PurchasedVariantId &&
                        oi.Order.UserId == userId &&
                        oi.Order.Status == OrderStatus.Delivered,
                    cancellationToken);
            if (!hasPurchased)
            {
                throw new BadRequestException(
                    "You can only review products you have purchased and received.");
            }
            var alreadyReviewed = await unitOfWork.Repository<Review>()
                .AnyAsync(Review => Review.UserId == userId && Review.PurchasedVariantId == request.PurchasedVariantId,
                cancellationToken);
            if (alreadyReviewed)
                throw new BadRequestException("User has already reviewed this variant.");

            var review = mapper.Map<Review>(request);
            review.UserId = userId;
            review.ProductId = variant.ProductId;
            await unitOfWork.Repository<Review>().AddAsync(review, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return review.Id;
        }
    }
}
