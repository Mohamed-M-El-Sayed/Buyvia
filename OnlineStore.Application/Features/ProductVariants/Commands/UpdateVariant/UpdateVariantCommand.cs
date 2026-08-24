using System.Text.Json.Serialization;
using MediatR;
using OnlineStore.Application.Contracts.Services.Caching;

namespace OnlineStore.Application.Features.ProductVariants.Commands.UpdateVariant
{
    [InvalidateCache(CacheTags.Products)]
    public class UpdateVariantCommand : IRequest
    {
        [JsonIgnore]
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int StockThreshold { get; set; }
        public bool IsActive { get; set; }
    }
}
