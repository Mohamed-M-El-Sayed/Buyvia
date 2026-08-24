using AutoMapper;
using MediatR;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.ProductOptions.Dtos;
using OnlineStore.Application.Features.ProductOptions.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductOptions.Queries.GetProductOptionsByProductId
{
    public class GetProductOptionsByProductIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : IRequestHandler<GetProductOptionsByProductIdQuery, List<ProductOptionDto>>
    {
        public async Task<List<ProductOptionDto>> Handle(
            GetProductOptionsByProductIdQuery request,
            CancellationToken cancellationToken)
        {
            var product = await unitOfWork.Repository<Product>()
                .GetEntityWithSpecAsync(
                    new ProductOptionsByProductIdSpecification(request.ProductId),
                    cancellationToken);

            if (product is null)
                throw new NotFoundException(
                    nameof(Product),
                    request.ProductId.ToString());

            return mapper.Map<List<ProductOptionDto>>(product.Options);
        }
    }
}