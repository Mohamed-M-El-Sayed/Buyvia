using MediatR;
using OnlineStore.Application.Features.Categories.Dto;

namespace OnlineStore.Application.Features.Categories.Queries.GetLeafCategories
{
    public class GetLeafCategoriesQuery(int rootId) : IRequest<List<CategorySummaryDto>>
    {
        public int RootId { get; } = rootId;
    }
}
