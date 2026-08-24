using MediatR;
using OnlineStore.Application.Contracts.Services.Caching;

namespace OnlineStore.Application.Features.Categories.Commands.CreateCategory
{
    [InvalidateCache(CacheTags.Categories)]
    public class CreateCategoryCommand : IRequest<int>
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public int? ParentId { get; set; }
        // null in case of parent category
    }
}
