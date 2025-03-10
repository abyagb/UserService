using AutoMapper;
using UserService.Api.ViewModels;
using UserService.Application.DTOs;

namespace UserService.Api.Mappings
{
    public class UserModelProfile : Profile
    {
        public UserModelProfile()
        {
            CreateMap<CreateUserViewModel, UserDto>();
        }
    }
}
