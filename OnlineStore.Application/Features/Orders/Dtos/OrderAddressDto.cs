namespace OnlineStore.Application.Features.Orders.Dtos
{
    public class OrderAddressDto
    {
        public string PhoneNumber { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Country { get; set; } = default!;
    }
}
