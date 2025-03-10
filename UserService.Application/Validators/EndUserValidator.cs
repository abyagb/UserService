using UserService.Application.DTOs;
using UserService.Repository.Interfaces;

namespace UserService.Application.Validators
{
    public  class EndUserValidator(IUserRepository userRepository) : IEndUserValidator
    {
        public async Task<(bool, string)> Validate(UserDto userDto)
        {
            if (await userRepository.CheckIfEmailExists(userDto.Email))
                return (false, "Email already exists in the system");

            if (await userRepository.CheckIfPhoneNumberExists(userDto.PhoneNumber))
                return (false, "Phone number already exists in the system");

            return (true, String.Empty);
        }
    }
}
