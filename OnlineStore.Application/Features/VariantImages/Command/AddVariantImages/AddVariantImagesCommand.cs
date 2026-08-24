using System.Text.Json.Serialization;
using MediatR;
using OnlineStore.Application.Contracts.Services.Caching;
using OnlineStore.Application.Features.VariantImages.Dtos;

namespace OnlineStore.Application.Features.VariantImages.Command.AddVariantImages
{
    [InvalidateCache(CacheTags.Products)]
    public class AddVariantImagesCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int VariantId { get; set; }
        public List<AddVariantImageDto> Images { get; set; } = [];
    }
}
