namespace OnlineStore.Application.Features.ProductOptions.Dtos
{
    public class ProductOptionDetailsDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; } = default!;
        public List<ProductOptionValueDto> Values { get; set; } = new();
    }
}
