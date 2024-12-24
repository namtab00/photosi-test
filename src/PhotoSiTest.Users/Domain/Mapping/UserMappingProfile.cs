using PhotoSiTest.Common.Mapping;
using PhotoSiTest.Contracts.Domain.Users.Dtos;

namespace PhotoSiTest.Users.Domain.Mapping;

public class UserMappingProfile : MappingProfileBase
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>();

        CreateMap<CreateUserDto, User>().ForMember(d => d.Id, opt => opt.Ignore());

        CreateMap<UpdateUserDto, User>().ForMember(d => d.Id, opt => opt.Ignore()).ForMember(d => d.Email, opt => opt.Ignore());
    }
}
