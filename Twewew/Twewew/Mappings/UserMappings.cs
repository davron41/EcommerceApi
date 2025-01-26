using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Twewew.DTOs;
using Twewew.Requests.User;

namespace Twewew.Mappings;

public class UserMappings : Profile
{
    public UserMappings()
    {
        CreateMap<IdentityUser<Guid>, UserDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
        CreateMap<UpdateUserRequest, IdentityUser<Guid>>();
    }
}
