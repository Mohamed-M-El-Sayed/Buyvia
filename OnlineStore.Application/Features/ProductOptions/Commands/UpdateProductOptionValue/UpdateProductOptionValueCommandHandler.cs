using MediatR;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductOptions.Commands.UpdateProductOptionValue
{
    public class UpdateProductOptionValueCommandHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<UpdateProductOptionValueCommand>
    {
        public async Task Handle(
            UpdateProductOptionValueCommand request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Value))
                throw new BadRequestException("Option value is required.");

            var optionValue = await unitOfWork.Repository<ProductOptionValue>()
                .FindAsync(
                    v => v.Id == request.Id,
                    cancellationToken)
                ?? throw new NotFoundException(
                    nameof(ProductOptionValue),
                    request.Id.ToString());

            var newValue = request.Value.Trim();

            var duplicateValue = await unitOfWork.Repository<ProductOptionValue>()
                .AnyAsync(
                    v => v.OptionId == optionValue.OptionId &&
                         v.Id != request.Id &&
                         v.Value == newValue,
                    cancellationToken);

            if (duplicateValue)
                throw new BadRequestException(
                    "An option value with the same value already exists for this option.");

            optionValue.Value = newValue;

            unitOfWork.Repository<ProductOptionValue>().Update(optionValue);

            await unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}