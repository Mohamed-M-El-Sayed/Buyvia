using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Categories.Dto;
using OnlineStore.Domain.Entities.Products;


namespace OnlineStore.Application.Features.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper,
        ILogger<GetCategoryByIdQueryHandler> logger) : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
    {
        public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation(
            "Getting category with Id {CategoryId}",
            request.CategoryId);
            var category = await unitOfWork
           .Repository<ProductCategory>().GetByIdAsync(request.CategoryId)
           ?? throw new NotFoundException(nameof(ProductCategory), request.CategoryId.ToString());
            return mapper.Map<CategoryDto>(category);
        }
    }
}
