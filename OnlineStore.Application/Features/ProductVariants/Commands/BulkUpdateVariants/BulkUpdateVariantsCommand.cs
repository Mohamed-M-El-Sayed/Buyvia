using MediatR;
using OnlineStore.Application.Contracts.Services.Caching;
using OnlineStore.Application.Features.ProductVariants.Dtos;

namespace OnlineStore.Application.Features.ProductVariants.Commands.BulkUpdateVariants
{
    [InvalidateCache(CacheTags.Products)]
    public class BulkUpdateVariantsCommand : IRequest<Unit>
    {
        public int ProductId { get; set; }
        public List<VariantUpdateItemDto> Variants { get; set; } = [];
    }
}
