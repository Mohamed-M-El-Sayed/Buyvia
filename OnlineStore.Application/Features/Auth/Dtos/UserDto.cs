namespace OnlineStore.Application.Features.Auth.Dtos
{
    public class UserDto
    {
        public string Id { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string ProfilePictureUrl { get; set; } = default!;
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
