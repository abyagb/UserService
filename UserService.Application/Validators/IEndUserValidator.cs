using UserService.Application.DTOs;

namespace UserService.Application.Validators
{
    public interface IEndUserValidator
    {
        Task<(bool, string)> Validate(UserDto userDto);
    }
}
