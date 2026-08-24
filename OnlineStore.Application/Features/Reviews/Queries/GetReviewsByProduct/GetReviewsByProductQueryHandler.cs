using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Common.Models;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Reviews.Dtos;
using OnlineStore.Application.Features.Reviews.Specifications;
using OnlineStore.Domain.Entities;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Reviews.Queries.GetReviewsByProduct
{
    public class GetReviewsByProductQueryHandler(IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetReviewsByProductQueryHandler> logger) : IRequestHandler<GetReviewsByProductQuery, PageResult<ReviewDto>>
    {
        public async Task<PageResult<ReviewDto>> Handle(GetReviewsByProductQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation(
                            "Getting reviews for product {ProductId}. Page {PageNumber}, Size {PageSize}",
                            request.ProductId,
                            request.PageNumber,
                            request.PageSize);
            var productExists = await unitOfWork.Repository<Product>()
                .AnyAsync(v => v.Id == request.ProductId, cancellationToken);
            if (!productExists)
                throw new NotFoundException(nameof(Product), request.ProductId.ToString());
            var totalCount = await unitOfWork.Repository<Review>().GetCountAsync(new ReviewsByProductSpecification(request, isPaginationEnabled: false), cancellationToken);

            var reviews = await unitOfWork.Repository<Review>().GetAllWithSpecAsync(new ReviewsByProductSpecification(request), cancellationToken);

            var result = mapper.Map<List<ReviewDto>>(reviews);
            return new PageResult<ReviewDto>(result, request.PageNumber, request.PageSize, totalCount);

        }
    }
}
