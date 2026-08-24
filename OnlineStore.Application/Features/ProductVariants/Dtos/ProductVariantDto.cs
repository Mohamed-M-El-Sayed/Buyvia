using OnlineStore.Application.Features.VariantImages.Dtos;

namespace OnlineStore.Application.Features.ProductVariants.Dtos
{
    // shared dto 
    public class ProductVariantDto
    {
        public int Id { get; set; }
        public string VariantName { get; set; } = default!;
        public decimal OriginalPrice { get; set; }
        public decimal FinalPrice { get; set; }
        public bool HasDiscount => FinalPrice < OriginalPrice;
        public int Stock { get; set; }
        public bool IsInStock => Stock > 0;
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public List<VariantImageDto> Images { get; set; } = [];
        public List<int> OptionValueIds { get; set; } = [];


    }
}
