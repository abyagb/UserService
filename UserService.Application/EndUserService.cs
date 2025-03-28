using AutoMapper;
using Microsoft.Extensions.Logging;
using UserService.Application.DTOs;
using UserService.Application.Exceptions;
using UserService.Application.Helpers;
using UserService.Application.Interfaces;
using UserService.Application.Validators;
using UserService.Domain;
using UserService.Repository.Interfaces;

namespace UserService.Application
{
    public class EndUserService(IUserRepository userRepository, IMapper mapper, IEndUserValidator endUserValidator, ILogger<EndUserService> logger) : IEndUserService
    {
        private readonly string EntityName = "User";

        public async Task CreateAsync(UserDto userDto)
        {
            var isValidNewUser = await endUserValidator.Validate(userDto);

            if (!isValidNewUser.Item1)
            {
                logger.LogError("An error occurred when creating a new user.Error message: {validationError}", isValidNewUser.Item2);
                throw new InvalidEntityException(EntityName, null, isValidNewUser.Item2);
            }

            var newUser = mapper.Map<User>(userDto);
            newUser.UserId = Guid.NewGuid();
            await userRepository.CreateAsync(newUser);
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid userId)
        {
            var user = await userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                logger.LogError("User not found. UserId : {userId}", userId);
                throw new EntityNotFoundException("User", userId);
            }

            var userDto = mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task DeleteAsync(Guid userId)
        {
            var user = await userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                logger.LogError("User not found. UserId : {userId}", userId);
                throw new EntityNotFoundException("User", userId);
            }

            await userRepository.DeleteAsync(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await userRepository.GetAllAsync();
            return mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task UpdateAsync(Guid userId, UserPatchDto userPatchDto)
        {
            var userToUpdate = await userRepository.GetUserByIdAsync(userId);

            if (userToUpdate == null)
            {
                logger.LogError("User not found. UserId : {userId}", userId);
                throw new EntityNotFoundException("User", userId);
            }

            EndUserServiceHelper.ValidatePatchFields(userPatchDto.FieldsToUpdate);
            EndUserServiceHelper.ApplyPatch(userToUpdate, userPatchDto.FieldsToUpdate); 
            
            // To do Add Validation for editing a user
            await userRepository.UpdateAsync(userToUpdate);
        }
    }
}
