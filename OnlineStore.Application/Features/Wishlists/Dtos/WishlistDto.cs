namespace OnlineStore.Application.Features.Wishlists.Dtos
{
    public class WishlistDto
    {
        public long Id { get; set; }
        public List<WishlistItemDto> Items { get; set; } = [];
    }
}
