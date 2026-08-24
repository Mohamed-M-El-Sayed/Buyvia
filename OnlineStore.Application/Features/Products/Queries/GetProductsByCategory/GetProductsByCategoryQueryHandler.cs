using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Common.Models;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Products.Dtos;
using OnlineStore.Application.Features.Products.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Products.Queries.GetProductsByCategory
{
    public class GetProductsByCategoryQueryHandler(IUnitOfWork unitOfWork,
        IMapper mapper, ILogger<GetProductsByCategoryQueryHandler> logger)
        : IRequestHandler<GetProductsByCategoryQuery, PageResult<ProductSummaryDto>>
    {

        public async Task<PageResult<ProductSummaryDto>> Handle(
      GetProductsByCategoryQuery request,
      CancellationToken cancellationToken)
        {
            // solve problem of filter based on final price 
            logger.LogInformation(
                "Getting products for CategoryId: {CategoryId}, Page: {PageNumber}, PageSize: {PageSize}",
                request.CategoryId, request.PageNumber, request.PageSize);

            var categoryIds = await unitOfWork.Categories
                .GetLeafCategoryIdsAsync(request.CategoryId, cancellationToken);

            if (!categoryIds.Any())
                throw new NotFoundException("Category", request.CategoryId.ToString());

            var countSpec = new ProductVariantByCategoryCountSpecification(
                categoryIds,
                request.MinPrice,
                request.MaxPrice,
                request.SearchTerm);

            var dataSpec = new ProductVariantByCategorySpecification(
                categoryIds,
                request.PageNumber,
                request.PageSize,
                request.MinPrice,
                request.MaxPrice,
                request.SortBy,
                request.SortDirection,
                request.SearchTerm);

            var totalCount = await unitOfWork.Repository<ProductVariant>()
                .GetCountAsync(countSpec, cancellationToken);

            var variants = await unitOfWork.Repository<ProductVariant>()
                .GetAllWithSpecAsync(dataSpec, cancellationToken);

            var result = mapper.Map<List<ProductSummaryDto>>(variants);
            logger.LogInformation(
                "Found {TotalCount} products for CategoryId: {CategoryId}, returning page {PageNumber}",
                totalCount, request.CategoryId, request.PageNumber);

            return new PageResult<ProductSummaryDto>(result, request.PageNumber, request.PageSize, totalCount);
        }



    }
}