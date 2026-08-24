using AutoMapper;
using MediatR;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler(IUnitOfWork unitOfWork,
        IMapper mapper) : IRequestHandler<UpdateProductCommand>
    {
        private const int MaxCategoryDepth = 3;

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await unitOfWork.Repository<Product>()
                .GetByIdAsync(request.Id)
                ?? throw new NotFoundException("Product", request.Id.ToString());

            var brand = await unitOfWork.Repository<ProductBrand>()
                .GetByIdAsync(request.BrandId)
                ?? throw new NotFoundException("Brand", request.BrandId.ToString());

            var categoryDepth = await unitOfWork.Categories
                .GetDepthAsync(request.CategoryId, cancellationToken);

            if (categoryDepth != MaxCategoryDepth)
                throw new BadRequestException(
                    $"Products can only be assigned to level {MaxCategoryDepth} categories.");

            mapper.Map(request, product);
            unitOfWork.Repository<Product>().Update(product);
            await unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}