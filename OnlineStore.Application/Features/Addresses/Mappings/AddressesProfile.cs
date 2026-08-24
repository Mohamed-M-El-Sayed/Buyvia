using AutoMapper;
using OnlineStore.Application.Features.Addresses.Commands.CreateAddress;
using OnlineStore.Application.Features.Addresses.Commands.UpdateAddress;
using OnlineStore.Application.Features.Addresses.Dtos;
using OnlineStore.Domain.Entities.Identity;

namespace OnlineStore.Application.Features.Addresses.Mappings
{
    public class AddressesProfile : Profile
    {
        public AddressesProfile()
        {
            CreateMap<CreateAddressCommand, UserAddress>();
            CreateMap<UserAddress, AddressDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
            CreateMap<UpdateAddressCommand, UserAddress>();
        }
    }
}
