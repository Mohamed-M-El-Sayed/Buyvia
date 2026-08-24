using System.Text.Json.Serialization;
using MediatR;
using OnlineStore.Application.Common.Models;
using OnlineStore.Application.Features.Reviews.Dtos;

namespace OnlineStore.Application.Features.Reviews.Queries.GetReviewsByProduct
{
    public class GetReviewsByProductQuery : IRequest<PageResult<ReviewDto>>
    {
        [JsonIgnore]
        public int ProductId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int? ExactRating { get; set; }
        public ReviewSortField SortBy { get; set; } = ReviewSortField.CreatedAt;
        public bool Descending { get; set; } = true;

    }
}