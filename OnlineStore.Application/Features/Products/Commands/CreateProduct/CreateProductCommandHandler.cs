using AutoMapper;
using MediatR;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Products;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : IRequestHandler<CreateProductCommand, int>
    {
        private const int MaxCategoryDepth = 3;

        public async Task<int> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            var brandExists = await unitOfWork
                .Repository<ProductBrand>()
                .AnyAsync(b => b.Id == request.BrandId);

            if (!brandExists)
            {
                throw new NotFoundException(
                    nameof(ProductBrand),
                    request.BrandId.ToString());
            }

            var categoryDepth = await unitOfWork.Categories
                .GetDepthAsync(
                    request.CategoryId,
                    cancellationToken);

            if (categoryDepth != MaxCategoryDepth)
            {
                throw new BadRequestException(
                    $"Products can only be assigned to level {MaxCategoryDepth} categories.");
            }

            var product = mapper.Map<Product>(request);

            product.Status = ProductStatus.Draft;

            await unitOfWork
                .Repository<Product>()
                .AddAsync(product, cancellationToken);

            await unitOfWork.CompleteAsync(cancellationToken);

            return product.Id;
        }
    }
}