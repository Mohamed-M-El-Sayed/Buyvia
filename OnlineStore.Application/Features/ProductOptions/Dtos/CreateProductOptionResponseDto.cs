namespace OnlineStore.Application.Features.ProductOptions.Dtos
{
    public class CreateProductOptionResponseDto
    {
        public int OptionId { get; set; }

        public bool HasExistingVariants { get; set; }

        public string? Warning { get; set; }

    }
}
