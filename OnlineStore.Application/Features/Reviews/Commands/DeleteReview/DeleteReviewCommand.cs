using MediatR;
using OnlineStore.Application.Contracts.Services.Caching;

namespace OnlineStore.Application.Features.Reviews.Commands.DeleteReview
{
    [InvalidateCache(CacheTags.Reviews)]
    public class DeleteReviewCommand(int reviewId) : IRequest<Unit>
    {
        public int ReviewId { get; } = reviewId;
    }
}
