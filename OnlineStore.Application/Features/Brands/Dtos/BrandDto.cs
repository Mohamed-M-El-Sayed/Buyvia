namespace OnlineStore.Application.Features.Brands.Dtos
{
    public class BrandDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string LogoUrl { get; set; } = default!;
    }
}
