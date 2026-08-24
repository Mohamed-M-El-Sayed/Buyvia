using MediatR;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Brands.Commands.CreateBrand
{
    public class CreateBrandCommandHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<CreateBrandCommand, int>
    {
        public async Task<int> Handle(
            CreateBrandCommand request,
            CancellationToken cancellationToken)
        {
            var brandRepository = unitOfWork.Repository<ProductBrand>();

            var exists = await brandRepository.AnyAsync(
                b => b.Name.ToLower() == request.Name.Trim().ToLower(),
                cancellationToken);

            if (exists)
            {
                throw new BadRequestException("A brand with the same name already exists.");
            }

            ProductBrand productBrand = new()
            {
                Name = request.Name.Trim(),
                LogoUrl = request.LogoUrl
            };

            await brandRepository.AddAsync(productBrand, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            return productBrand.Id;
        }
    }
}