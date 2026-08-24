using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Specifications;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Categories.Dto;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Categories.Queries.GetTopLevelCategories
{
    public class GetTopLevelCategoriesQueryHandler(IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetTopLevelCategoriesQueryHandler> logger) : IRequestHandler<GetTopLevelCategoriesQuery, IEnumerable<CategorySummaryDto>>
    {
        public async Task<IEnumerable<CategorySummaryDto>> Handle(GetTopLevelCategoriesQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting top level categories");
            var categories = await unitOfWork.Repository<ProductCategory>().GetAllWithSpecAsync(new BaseSpecification<ProductCategory> { Criteria = c => c.ParentId == null }, cancellationToken);
            var result = mapper.Map<IEnumerable<CategorySummaryDto>>(categories);
            return result;
        }
    }
}
