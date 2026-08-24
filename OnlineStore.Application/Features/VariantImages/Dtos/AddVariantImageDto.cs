namespace OnlineStore.Application.Features.VariantImages.Dtos
{
    public class AddVariantImageDto
    {

        public string ImageUrl { get; set; } = default!;

        public bool IsMainImage { get; set; }

    }
}
