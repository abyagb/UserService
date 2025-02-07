using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.DTOs;
using UserService.Domain;

namespace UserService.Application.Mappings
{
    public class UserDtoToUser:Profile
    {
        public UserDtoToUser() 
        {
            CreateMap<UserDto, User>();
        }
    }
}
