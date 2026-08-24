namespace OnlineStore.Application.Features.ProductOptions.Dtos
{
    public class ProductOptionDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; } = default!;
        //public int DisplayOrder { get; set; }
    }
}
