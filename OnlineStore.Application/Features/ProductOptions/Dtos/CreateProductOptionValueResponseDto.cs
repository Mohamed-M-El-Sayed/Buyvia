namespace OnlineStore.Application.Features.ProductOptions.Dtos
{
    public class CreateProductOptionValueResponseDto
    {
        public int OptionValueId { get; set; }
        public bool HasExistingVariants { get; set; }
        public string? Warning { get; set; }
    }
}
