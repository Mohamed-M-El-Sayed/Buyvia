using MediatR;
using OnlineStore.Application.Contracts.Services.Caching;

namespace OnlineStore.Application.Features.Products.Commands.CreateProduct
{
    [InvalidateCache(CacheTags.Products)]
    public class CreateProductCommand : IRequest<int>
    {
        public string Name { get; set; } = default!;
        public string ShortDescription { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
    }




}