namespace OnlineStore.Application.Features.ProductVariants.Dtos
{
    public class AdminProductVariantsDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;

        public int TotalVariants => Variants.Count;

        public List<AdminVariantDto> Variants { get; set; } = [];
    }

    public class AdminVariantDto
    {
        public int Id { get; set; }

        public string VariantName { get; set; } = string.Empty;

        public List<VariantOptionValueDto> OptionValues { get; set; } = [];

        public decimal OriginalPrice { get; set; }
        public decimal FinalPrice { get; set; }
        public bool HasDiscount { get; set; }

        public int Stock { get; set; }
        public bool IsInStock => Stock > 0;

        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
    }

    public class VariantOptionValueDto
    {
        public int OptionId { get; set; }
        public string OptionName { get; set; } = string.Empty;

        public int OptionValueId { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}
