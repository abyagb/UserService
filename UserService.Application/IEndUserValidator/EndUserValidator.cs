using System;
using System.Collections.Generic;
using System.Linq;
using UserService.Repository.Interfaces;

namespace UserService.Application.IEndUserValidator
{
    public  class EndUserValidator(IUserRepository userRepository)
    {
        public async Task<bool> CheckIfUserExists(string email,string phoneNumber)
        {
            return await userRepository.CheckIfUserExists(email, phoneNumber);
        }
    }
}
