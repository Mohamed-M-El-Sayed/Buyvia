using OnlineStore.Application.Features.ProductVariants.Dtos;

namespace OnlineStore.Application.Features.Products.Dtos
{
    public class ProductDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string ShortDescription { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string BrandName { get; set; } = default!;
        public string CategoryName { get; set; } = default!;
        public bool IsSimple { get; set; }
        // if product has no options 
        public List<ProductOptionDto> Options { get; set; } = [];
        public List<ProductVariantDto> Variants { get; set; } = [];

    }
    public class ProductOptionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;              // "Color"
        public List<ProductOptionValueDto> Values { get; set; } = [];
    }
    public class ProductOptionValueDto
    {
        public int Id { get; set; }
        public string Value { get; set; } = default!;             // "Red"
    }
}



