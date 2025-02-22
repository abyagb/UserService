using AutoMapper;
using UserService.Application.DTOs;
using UserService.Application.Exceptions;
using UserService.Application.IEndUserValidator;
using UserService.Application.Interfaces;
using UserService.Domain;
using UserService.Repository.Interfaces;

namespace UserService.Application
{
    public class EndUserService(IUserRepository userRepository, IMapper mapper) : IEndUserService
    {
        public async Task CreateAsync(UserDto userDto)
        {
            var endUserValidator = new EndUserValidator(userRepository);
            var userExists = await endUserValidator.CheckIfUserExists(userDto.Email, userDto.PhoneNumber);
            if (userExists)
            {
                throw new InvalidUserException($"User with email {userDto.Email} or phone number {userDto.PhoneNumber} already exists.");

            }
            var user = mapper.Map<User>(userDto);
            user.UserId = Guid.NewGuid();
            await userRepository.CreateAsync(user);
        }

        public async Task DeleteAsync(Guid userId)
        {
            var user = await userRepository.GetUserByIdAsync(userId);
            if(user == null)
            {
                throw new KeyNotFoundException($"User with Id:{userId} not found");
            }
            await userRepository.DeleteAsync(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await userRepository.GetAllAsync();
            return mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid userId)
        {
            var user = await userRepository.GetUserByIdAsync(userId);
            if(user == null)
            {
                throw new KeyNotFoundException($"User with Id:{userId} not found");
            }
            var userDto=mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task UpdateAsync(Guid id, UserDto userDto)
        {
            var user = mapper.Map<User>(userDto);
            user.UserId = id;
            await userRepository.UpdateAsync(user);
        }
        
        
    }
}
