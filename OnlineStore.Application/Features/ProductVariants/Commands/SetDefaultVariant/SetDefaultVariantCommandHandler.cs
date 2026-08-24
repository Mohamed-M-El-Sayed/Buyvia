using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.ProductVariants.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductVariants.Commands.SetDefaultVariant
{
    public class SetDefaultVariantCommandHandler(IUnitOfWork unitOfWork,
        ILogger<SetDefaultVariantCommandHandler> logger) : IRequestHandler<SetDefaultVariantCommand, Unit>
    {
        public async Task<Unit> Handle(SetDefaultVariantCommand request, CancellationToken cancellationToken)
        {
            // add condition if simple product cannot edit is deafult 
            logger.LogInformation("Setting variant {variantId} as default", request.VariationId);
            var existingVariant = await unitOfWork.Repository<ProductVariant>()
               .GetByIdAsync(request.VariationId)
               ?? throw new NotFoundException(nameof(ProductVariant), request.VariationId.ToString());
            // already default variant
            if (existingVariant.IsDefault)
                return Unit.Value;

            if (!existingVariant.IsActive)
                throw new BadRequestException(
                "Cannot set an inactive variant as default. Activate it first.");
            var variants = await unitOfWork.Repository<ProductVariant>()
              .GetAllWithSpecAsync(new VariantsByProductIdSpecification(existingVariant.ProductId, null));

            await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                foreach (var variant in variants)
                    variant.IsDefault = false;
                await unitOfWork.CompleteAsync(cancellationToken);

                existingVariant.IsDefault = true;
                await unitOfWork.CompleteAsync(cancellationToken);

                await unitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
            logger.LogInformation(
            "Variant {VariantId} is now the default for product {ProductId}",
            request.VariationId, existingVariant.ProductId);
            return Unit.Value;


        }
    }


}
