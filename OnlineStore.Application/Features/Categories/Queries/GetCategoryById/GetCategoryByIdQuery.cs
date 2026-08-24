using MediatR;
using OnlineStore.Application.Features.Categories.Dto;

namespace OnlineStore.Application.Features.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQuery(int categoryId) : IRequest<CategoryDto>
    {
        public int CategoryId { get; } = categoryId;
    }
}
