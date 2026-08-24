using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Categories.Dto;
using OnlineStore.Application.Features.Categories.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Categories.Queries.GetCategoryTree
{
    public class GetCategoryTreeQueryHandler(IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetCategoryTreeQueryHandler> logger) : IRequestHandler<GetCategoryTreeQuery, IEnumerable<CategoryTreeDto>>
    {
        public async Task<IEnumerable<CategoryTreeDto>> Handle(GetCategoryTreeQuery request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Retrieving category tree.");
            var categories = await unitOfWork.Repository<ProductCategory>()
            .GetAllWithSpecAsync(
                new CategoryTreeSpecification(),
                cancellationToken);

            return mapper.Map<IEnumerable<CategoryTreeDto>>(categories);
        }
    }
}
