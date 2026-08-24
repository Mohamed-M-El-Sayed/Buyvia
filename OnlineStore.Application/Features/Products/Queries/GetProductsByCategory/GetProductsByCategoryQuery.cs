using System.Text.Json.Serialization;
using MediatR;
using OnlineStore.Application.Common.Models;
using OnlineStore.Application.Features.Products.Dtos;

namespace OnlineStore.Application.Features.Products.Queries.GetProductsByCategory
{
    public class GetProductsByCategoryQuery : IRequest<PageResult<ProductSummaryDto>>
    {
        [JsonIgnore]
        public int CategoryId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public ProductSortField? SortBy { get; set; }
        public SortDirection? SortDirection { get; set; }

        // Filters
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SearchTerm { get; set; }
    }
}
