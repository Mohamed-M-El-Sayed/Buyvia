using AutoMapper;
using MediatR;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.ProductOptions.Dtos;
using OnlineStore.Application.Features.ProductOptions.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductOptions.Queries.GetProductOptionById
{
    public class GetProductOptionByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : IRequestHandler<GetProductOptionByIdQuery, ProductOptionDetailsDto>
    {
        public async Task<ProductOptionDetailsDto> Handle(
            GetProductOptionByIdQuery request,
            CancellationToken cancellationToken)
        {
            var option = await unitOfWork.Repository<ProductOption>()
                .GetEntityWithSpecAsync(
                    new ProductOptionByIdSpecification(request.OptionId),
                    cancellationToken)
                ?? throw new NotFoundException(
                    nameof(ProductOption),
                    request.OptionId.ToString());

            return mapper.Map<ProductOptionDetailsDto>(option);
        }
    }
}