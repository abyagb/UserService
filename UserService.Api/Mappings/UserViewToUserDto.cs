using AutoMapper;
using UserService.Api.ViewModels;
using UserService.Application.DTOs;

namespace UserService.Api.Mappings
{
    //instead of putting the mappings folder in the UserService.Api project, it needs to be in the shared project because it is 
    //referenced by both the UserService.Api and UserService.Application projects
    //rename the 'Mappings' folder to 'MappingProfiles'
    //and rename the 'UserViewToUserDto.cs' file to 'UserMappingProfile.cs'
    public class UserViewToUserDto:Profile // there needs to be spaces between :
    {
        public UserViewToUserDto()
        {
            CreateMap<CreateUserViewModel,UserDto>();
        }
    }
}
