using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.ProductVariants.Dtos;
using OnlineStore.Application.Features.ProductVariants.Queries.GetVariantsByProductId;
using OnlineStore.Application.Features.ProductVariants.Specifications;
using OnlineStore.Domain.Entities.Products;

public class GetVariantsByProductIdQueryHandler(IUnitOfWork unitOfWork,
    ILogger<GetVariantsByProductIdQueryHandler> logger,
    IMapper mapper) : IRequestHandler<GetVariantsByProductIdQuery, AdminProductVariantsDto>
{
    public async Task<AdminProductVariantsDto> Handle(GetVariantsByProductIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting Product Variants for ProductId: {ProductId}", request.ProductId);

        var variants = await unitOfWork.Repository<ProductVariant>()
            .GetAllWithSpecAsync(new VariantsByProductIdSpecification(request.ProductId), cancellationToken);
        return new AdminProductVariantsDto
        {
            ProductId = request.ProductId,
            ProductName = variants.FirstOrDefault()?.Product?.Name ?? string.Empty,
            Variants = mapper.Map<List<AdminVariantDto>>(variants)
        };
    }
}