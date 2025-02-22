using AutoMapper;
using UserService.Application.DTOs;
using UserService.Domain;

namespace UserService.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<UserDto, User>().ReverseMap();
        }
    }
}
