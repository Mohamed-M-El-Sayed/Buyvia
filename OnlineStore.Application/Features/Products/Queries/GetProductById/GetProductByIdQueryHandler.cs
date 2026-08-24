using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Products.Dtos;
using OnlineStore.Application.Features.Products.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetProductByIdQueryHandler> logger,
        IMapper mapper
        ) : IRequestHandler<GetProductByIdQuery, ProductDetailsDto>
    {
        public async Task<ProductDetailsDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation(
                        "Getting product details for ProductId: {ProductId}", request.ProductId);

            var product = await unitOfWork.Repository<Product>()
                .GetEntityWithSpecAsync(
                    new ProductDetailsByIdSpecification(request.ProductId),
                    cancellationToken)
                ?? throw new NotFoundException(nameof(Product), request.ProductId.ToString());

            var productDto = mapper.Map<ProductDetailsDto>(product);

            return productDto;
        }
    }
}
