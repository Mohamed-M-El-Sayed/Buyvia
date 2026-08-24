using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Reviews.Dtos;
using OnlineStore.Application.Features.Reviews.Specifications;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Application.Features.Reviews.Queries.GetReviewById
{
    public class GetReviewByIdQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetReviewByIdQueryHandler> logger,
        IMapper mapper) : IRequestHandler<GetReviewByIdQuery, ReviewDto>
    {
        public async Task<ReviewDto> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting review with Id: {ReviewId}", request.Id);
            var review = await unitOfWork.Repository<Review>()
                .GetEntityWithSpecAsync(new ReviewByIdSpecification(request.Id), cancellationToken)
                ?? throw new NotFoundException(nameof(Review), request.Id.ToString());
            var result = mapper.Map<ReviewDto>(review);
            return result;
        }
    }
}