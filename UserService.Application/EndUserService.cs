using AutoMapper;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Domain;
using UserService.Repository.Interfaces;

namespace UserService.Application
{
    public class EndUserService : IEndUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public EndUserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        private async Task<bool> CheckIfUserExistsByEmailAsync(string email)
        {
            return await _userRepository.CheckIfUserExists(email);
        }

        public async Task CreateAsync(UserDto userDto)
        {
            var userExists = await CheckIfUserExistsByEmailAsync(userDto.Email);
            if (userExists)
            {
                throw new Exception("User already exists");
            }

            var user = _mapper.Map<User>(userDto);
            user.UserId = Guid.NewGuid();
            await _userRepository.CreateAsync(user);
        }

        public async Task DeleteAsync(Guid userId)
        {
            var user=await _userRepository.GetUserByIdAsync(userId);
            if(user == null)
            {
                throw new KeyNotFoundException($"User with Id:{userId} not found");
            }
            await _userRepository.DeleteAsync(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if(user==null)
            {
                throw new KeyNotFoundException($"User with Id:{userId} not found");
            }
            var userDto=_mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task UpdateAsync(Guid id, UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.UserId = id;
            await _userRepository.UpdateAsync(user);
        }
    }
}
