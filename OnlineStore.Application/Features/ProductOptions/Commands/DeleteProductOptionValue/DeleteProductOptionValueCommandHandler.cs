using MediatR;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductOptions.Commands.DeleteProductOptionValue
{
    public class DeleteProductOptionValueCommandHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<DeleteProductOptionValueCommand>
    {
        public async Task Handle(DeleteProductOptionValueCommand request, CancellationToken cancellationToken)
        {
            var optionValue = await unitOfWork.Repository<ProductOptionValue>()
                .FindAsync(v => v.Id == request.ValueId, cancellationToken)
                ?? throw new NotFoundException(nameof(ProductOptionValue), request.ValueId.ToString());

            var isUsedByVariant = await unitOfWork.Repository<VariantOption>()
                .AnyAsync(vo => vo.OptionValueId == request.ValueId, cancellationToken);

            if (isUsedByVariant)
                throw new BadRequestException("Cannot delete an option value that is used by product variants.");

            unitOfWork.Repository<ProductOptionValue>().Delete(optionValue);
            await unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
