namespace OnlineStore.Application.Features.Products.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string ShortDescription { get; set; } = default!;
        public string BrandName { get; set; } = default!;
        public string CategoryName { get; set; } = default!;
    }
}

