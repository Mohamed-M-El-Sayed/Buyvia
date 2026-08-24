namespace OnlineStore.Application.Features.ProductOptions.Dtos
{
    public class ProductOptionValueDto
    {
        public int Id { get; set; }
        public int OptionId { get; set; }
        public string Value { get; set; } = default!;
    }
}
