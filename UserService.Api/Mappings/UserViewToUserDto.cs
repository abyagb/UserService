using AutoMapper;
using UserService.Api.ViewModels;
using UserService.Application.DTOs;

namespace UserService.Api.Mappings
{
    public class UserViewToUserDto:Profile
    {
        public UserViewToUserDto()
        {
            CreateMap<CreateUserViewModel,UserDto>();
        }
    }
}
