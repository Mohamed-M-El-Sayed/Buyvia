using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Application.Features.Reviews.Commands.DeleteReview
{
    public class DeleteReviewCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        ILogger<DeleteReviewCommandHandler> logger)
        : IRequestHandler<DeleteReviewCommand, Unit>
    {
        public async Task<Unit> Handle(
            DeleteReviewCommand request,
            CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId
                ?? throw new UnauthorizedException(
                    "User must be authenticated.");

            logger.LogInformation(
                "User {UserId} deleting review {ReviewId}",
                userId,
                request.ReviewId);

            var review = await unitOfWork.Repository<Review>()
                .GetByIdAsync(request.ReviewId)
                ?? throw new NotFoundException(
                    nameof(Review),
                    request.ReviewId.ToString());

            if (!currentUserService.IsAdmin &&
                review.UserId != userId)
            {
                throw new BadRequestException(
                    "You can only delete your own reviews.");
            }

            review.Delete();

            await unitOfWork.CompleteAsync(cancellationToken);

            logger.LogInformation(
                "Review {ReviewId} deleted successfully.",
                request.ReviewId);

            return Unit.Value;
        }
    }
}