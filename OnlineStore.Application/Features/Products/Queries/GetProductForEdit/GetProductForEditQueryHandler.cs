using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Products.Dtos;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Products.Queries.GetProductForEdit
{
    public class GetProductForEditQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetProductForEditQueryHandler> logger,
        IMapper mapper) : IRequestHandler<GetProductForEditQuery, ProductEditDto>
    {

        public async Task<ProductEditDto> Handle(GetProductForEditQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting product for edit: {ProductId}", request.ProductId);
            var product = await unitOfWork.Repository<Product>()
            .GetByIdAsync(request.ProductId)
            ?? throw new NotFoundException(nameof(Product), request.ProductId.ToString());
            return mapper.Map<ProductEditDto>(product);
        }
    }
}
