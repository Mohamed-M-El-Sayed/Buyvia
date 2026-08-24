using MediatR;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductOptions.Commands.DeleteProductOption
{
    public class DeleteProductOptionCommandHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<DeleteProductOptionCommand>
    {
        public async Task Handle(DeleteProductOptionCommand request, CancellationToken cancellationToken)
        {
            var option = await unitOfWork.Repository<ProductOption>()
                .GetByIdAsync(request.OptionId)
                ?? throw new NotFoundException(nameof(ProductOption), request.OptionId.ToString());

            var isUsedByVariant = await unitOfWork.Repository<VariantOption>()
                .AnyAsync(vo => vo.OptionId == request.OptionId, cancellationToken);

            if (isUsedByVariant)
                throw new BadRequestException("Cannot delete an option that is used by product variants.");

            unitOfWork.Repository<ProductOption>().Delete(option);
            await unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
