using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Categories.Dto;
using OnlineStore.Application.Features.Categories.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Categories.Queries.GetLeafCategories;

public class GetLeafCategoriesQueryHandler(
    IUnitOfWork unitOfWork,
    ILogger<GetLeafCategoriesQueryHandler> logger,
    IMapper mapper)
    : IRequestHandler<GetLeafCategoriesQuery, List<CategorySummaryDto>>
{
    public async Task<List<CategorySummaryDto>> Handle(
        GetLeafCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Getting leaf categories for root category with ID {RootId}.",
            request.RootId);

        var categories = await unitOfWork
            .Repository<ProductCategory>()
            .GetAllWithSpecAsync(
                new LeafCategoriesByRootSpecification(request.RootId));

        var categoryDtos = mapper.Map<List<CategorySummaryDto>>(categories);
        return categoryDtos;
    }
}