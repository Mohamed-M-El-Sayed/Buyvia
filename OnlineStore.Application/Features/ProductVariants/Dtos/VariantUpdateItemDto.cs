namespace OnlineStore.Application.Features.ProductVariants.Dtos
{
    public class VariantUpdateItemDto
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int StockThreshold { get; set; }
        public bool IsActive { get; set; }
    }
}
