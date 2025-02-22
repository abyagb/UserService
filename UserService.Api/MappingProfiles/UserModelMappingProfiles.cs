using AutoMapper;
using UserService.Api.ViewModels;
using UserService.Application.DTOs;

namespace UserService.Api.Mappings
{
    public class UserModelMappingProfiles : Profile
    {
        public UserModelMappingProfiles()
        {
            CreateMap<CreateUserViewModel, UserDto>();
        }
    }
}
