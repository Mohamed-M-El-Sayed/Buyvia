using System.Text.Json.Serialization;
using MediatR;
using OnlineStore.Application.Contracts.Services.Caching;

namespace OnlineStore.Application.Features.Products.Commands.UpdateProduct
{
    [InvalidateCache(CacheTags.Products)]
    public class UpdateProductCommand : IRequest
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Name { get; set; } = default!;
        public string ShortDescription { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int BrandId { get; set; } = default!;
        public int CategoryId { get; set; } = default!;
    }
}
