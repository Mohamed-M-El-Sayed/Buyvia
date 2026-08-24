namespace OnlineStore.Application.Features.Categories.Dto
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int? ParentId { get; set; }
        //public List<CategoryDto> SubCategories { get; set; } = new();

    }
}
