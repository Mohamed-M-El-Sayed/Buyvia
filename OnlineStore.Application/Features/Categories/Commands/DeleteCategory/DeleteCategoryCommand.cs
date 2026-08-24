using MediatR;
using OnlineStore.Application.Contracts.Services.Caching;

namespace OnlineStore.Application.Features.Categories.Commands.DeleteCategory
{
    [InvalidateCache(CacheTags.Categories)]
    public class DeleteCategoryCommand(int id) : IRequest<Unit>
    {
        public int Id { get; } = id;
    }
}
