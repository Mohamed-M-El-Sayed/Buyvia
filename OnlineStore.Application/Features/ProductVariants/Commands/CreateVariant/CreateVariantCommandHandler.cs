using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.ProductVariants.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductVariants.Commands.AddVariant
{
    public class CreateVariantCommandHandler(IUnitOfWork unitOfWork,
        ILogger<CreateVariantCommandHandler> logger,
        IMapper mapper) : IRequestHandler<CreateVariantCommand, int>
    {
        public async Task<int> Handle(CreateVariantCommand request, CancellationToken cancellationToken)
        {
            // create variant for simple products only (no options)

            logger.LogInformation("Adding new variant to product {ProductId}", request.ProductId);

            var product = await unitOfWork.Repository<Product>().GetByIdAsync(request.ProductId)
                ?? throw new NotFoundException(nameof(Product), request.ProductId.ToString());

            // get varaint with options 
            var existingVariant = await unitOfWork.Repository<ProductVariant>()
              .GetEntityWithSpecAsync(new VariantWithOptionsSpecification(request.ProductId));
            // get vaatint wtih options 

            if (existingVariant != null && !existingVariant.Options.Any())
                throw new BadRequestException("Product already has simple variant");

            if (existingVariant != null && existingVariant.Options.Any())
                throw new BadRequestException("Cannot add a simple variant to a product that already has variant options.");

            var variant = mapper.Map<ProductVariant>(request);
            await unitOfWork.Repository<ProductVariant>().AddAsync(variant, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return variant.Id;
        }
    }
}
