using System.Text.Json.Serialization;
using MediatR;
using OnlineStore.Application.Contracts.Services.Caching;

namespace OnlineStore.Application.Features.Reviews.Commands.UpdateReview
{
    [InvalidateCache(CacheTags.Reviews)]
    public class UpdateReviewCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int ReviewId { get; set; }
        public int Rating { get; set; }
        public string? Title { get; set; }
        public string Comment { get; set; } = default!;
    }
}
