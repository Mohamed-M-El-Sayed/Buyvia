namespace OnlineStore.Application.Features.VariantImages.Dtos
{
    public class VariantImageDto
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; } = default!;

        public bool IsMainImage { get; set; }

        public int DisplayOrder { get; set; }
    }
}
