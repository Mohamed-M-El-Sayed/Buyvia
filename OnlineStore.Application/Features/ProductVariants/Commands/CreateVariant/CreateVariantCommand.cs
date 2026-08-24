using System.Text.Json.Serialization;
using MediatR;
using OnlineStore.Application.Contracts.Services.Caching;

namespace OnlineStore.Application.Features.ProductVariants.Commands.AddVariant
{
    [InvalidateCache(CacheTags.Products)]
    public class CreateVariantCommand : IRequest<int>
    {
        [JsonIgnore]
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int StockThreshold { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
