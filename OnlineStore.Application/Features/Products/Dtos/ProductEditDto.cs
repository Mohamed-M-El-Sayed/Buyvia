namespace OnlineStore.Application.Features.Products.Dtos
{
    public class ProductEditDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string ShortDescription { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
    }
}
