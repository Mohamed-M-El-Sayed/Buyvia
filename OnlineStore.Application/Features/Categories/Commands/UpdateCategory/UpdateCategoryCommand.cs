using System.Text.Json.Serialization;
using MediatR;
using OnlineStore.Application.Contracts.Services.Caching;

namespace OnlineStore.Application.Features.Categories.Commands.UpdateCategory
{
    [InvalidateCache(CacheTags.Categories)]
    public class UpdateCategoryCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
