using MediatR;
using OnlineStore.Application.Features.Categories.Dto;

namespace OnlineStore.Application.Features.Categories.Queries.GetTopLevelCategories
{
    public class GetTopLevelCategoriesQuery : IRequest<IEnumerable<CategorySummaryDto>>
    {
    }
}
