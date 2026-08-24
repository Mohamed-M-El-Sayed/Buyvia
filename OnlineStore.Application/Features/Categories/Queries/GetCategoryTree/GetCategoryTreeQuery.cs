using MediatR;
using OnlineStore.Application.Features.Categories.Dto;

namespace OnlineStore.Application.Features.Categories.Queries.GetCategoryTree
{
    public class GetCategoryTreeQuery() : IRequest<IEnumerable<CategoryTreeDto>>
    { }
}
