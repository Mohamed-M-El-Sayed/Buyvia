using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Application.Features.Reviews.Commands.UpdateReview
{
    public class UpdateReviewCommandHandler(IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UpdateReviewCommandHandler> logger,
        ICurrentUserService currentUserService) : IRequestHandler<UpdateReviewCommand, Unit>
    {

        public async Task<Unit> Handle(
            UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId
                ?? throw new UnauthorizedException("User must be authenticated.");

            logger.LogInformation(
                "User {UserId} updating review {ReviewId}",
                userId, request.ReviewId);

            var review = await unitOfWork.Repository<Review>()
                .GetByIdAsync(request.ReviewId)
                ?? throw new NotFoundException(nameof(Review), request.ReviewId.ToString());

            if (review.UserId != userId)
                throw new UnauthorizedAccessException(
                    "You are not allowed to update this review.");

            review = mapper.Map(request, review);
            await unitOfWork.CompleteAsync(cancellationToken);

            logger.LogInformation(
                "Review {ReviewId} updated successfully", request.ReviewId);
            return Unit.Value;
        }

    }
}
