namespace OnlineStore.Application.Features.Categories.Dto
{
    public class CategoryTreeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public List<CategoryTreeDto> Children { get; set; } = new();
    }
}
