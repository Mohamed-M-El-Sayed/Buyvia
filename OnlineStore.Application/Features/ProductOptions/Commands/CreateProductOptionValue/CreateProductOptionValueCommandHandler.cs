using MediatR;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.ProductOptions.Dtos;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductOptions.Commands.CreateProductOptionValue
{
    public class CreateProductOptionValueCommandHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<CreateProductOptionValueCommand, CreateProductOptionValueResponseDto>
    {
        public async Task<CreateProductOptionValueResponseDto> Handle(
            CreateProductOptionValueCommand request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Value))
                throw new BadRequestException("Option value is required.");

            var option = await unitOfWork.Repository<ProductOption>()
                .GetByIdAsync(request.OptionId)
                ?? throw new NotFoundException(nameof(ProductOption), request.OptionId.ToString());

            var trimmedValue = request.Value.Trim();

            var duplicateValue = await unitOfWork.Repository<ProductOptionValue>()
                .AnyAsync(
                    v => v.OptionId == request.OptionId && v.Value == trimmedValue,
                    cancellationToken);

            if (duplicateValue)
                throw new BadRequestException(
                    "An option value with the same value already exists for this option.");

            var hasExistingVariants = await unitOfWork.Repository<ProductVariant>()
                .AnyAsync(
                    v => v.ProductId == option.ProductId,
                    cancellationToken);

            var optionValue = new ProductOptionValue
            {
                OptionId = request.OptionId,
                Value = trimmedValue,
            };

            await unitOfWork.Repository<ProductOptionValue>()
                .AddAsync(optionValue, cancellationToken);

            await unitOfWork.CompleteAsync(cancellationToken);

            return new CreateProductOptionValueResponseDto
            {
                OptionValueId = optionValue.Id,
                HasExistingVariants = hasExistingVariants,
                Warning = hasExistingVariants
                    ? "This product already has variants. Generate variants again to include the new option value."
                    : null
            };
        }
    }
}