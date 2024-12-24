using PhotoSiTest.Common.Mapping;
using PhotoSiTest.Contracts.Domain.Addresses.Dtos;

namespace PhotoSiTest.Addresses.Domain.Mapping;

public class AddressMappingProfile : MappingProfileBase
{
    public AddressMappingProfile()
    {
        CreateMap<Address, AddressDto>();

        CreateMap<CreateAddressDto, Address>().ForMember(d => d.Id, opt => opt.Ignore());

        CreateMap<UpdateAddressDto, Address>().ForMember(d => d.Id, opt => opt.Ignore()).ForMember(d => d.UserId, opt => opt.Ignore());
    }
}
