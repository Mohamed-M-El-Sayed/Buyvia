using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Brands.Commands.UpdateBrand
{
    public class UpdateBrandCommandHandler(IUnitOfWork unitOfWork,
        ILogger<UpdateBrandCommandHandler> logger, IMapper mapper) : IRequestHandler<UpdateBrandCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var brandRepository = unitOfWork.Repository<ProductBrand>();

            var brand = await brandRepository.GetByIdAsync(request.Id)
             ?? throw new NotFoundException(nameof(ProductBrand), request.Id.ToString());

            var nameExists = await brandRepository.AnyAsync(
                b => b.Id != request.Id &&
                     b.Name.ToLower() == request.Name.ToLower(),
                cancellationToken);

            if (nameExists)
                throw new BadRequestException($"Brand with name '{request.Name}' already exists.");
            brand = mapper.Map(request, brand);
            brandRepository.Update(brand);
            await unitOfWork.CompleteAsync(cancellationToken);
            logger.LogInformation("Brand '{BrandName}' updated successfully.", brand.Name);
            return Unit.Value;

        }
    }
}
