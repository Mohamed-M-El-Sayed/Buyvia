
namespace OnlineStore.Application.Features.Products.Dtos
{
    public class ProductSummaryDto
    {
        public int Id { get; set; }
        public int DefaultVariantId { get; set; }
        public string ProductName { get; set; } = default!;
        public string VariantName { get; set; } = default!;
        public string ShortDescription { get; set; } = default!;
        public decimal OriginalPrice { get; set; }
        public decimal FinalPrice { get; set; }
        public bool HasDiscount { get; set; }
        public bool InStock { get; set; }
        public string BrandName { get; set; } = default!;
        public int BrandId { get; set; }
        public string CategoryName { get; set; } = default!;
        public int CategoryId { get; set; }
        public string MainImageUrl { get; set; } = default!;


    }
}
