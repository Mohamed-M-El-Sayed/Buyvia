using MediatR;
using OnlineStore.Application.Features.Reviews.Dtos;

namespace OnlineStore.Application.Features.Reviews.Queries.GetReviewById
{
    public class GetReviewByIdQuery(int id) : IRequest<ReviewDto>
    {
        public int Id { get; } = id;
    }
}
