using MediatR;
using OnlineStore.Application.Contracts.Services.Caching;

namespace OnlineStore.Application.Features.Reviews.Commands.CreateReview
{
    [InvalidateCache(CacheTags.Reviews)]
    public class CreateReviewCommand : IRequest<int>
    {
        public int PurchasedVariantId { get; set; }
        public int Rating { get; set; }
        public string? Title { get; set; }
        public string Comment { get; set; } = default!;
    }
}
