using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; //remove all the unecessary using statements
using UserService.Application.DTOs;
using UserService.Domain;

//this needs to be in the shared project because it is referenced by both the UserService.Api and UserService.Application projects
namespace UserService.Application.Mappings
{
    public class UserDtoToUser:Profile // there needs to be spaces between :
    {
        public UserDtoToUser() 
        {
            CreateMap<UserDto, User>();
        }
    }
}
