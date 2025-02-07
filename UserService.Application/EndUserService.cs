using AutoMapper;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Domain;
using UserService.Repository.Interfaces;

namespace UserService.Application
{
    public class EndUserService(IUserRepository userRepository,IMapper mapper) : IEndUserService
    {
        private readonly IUserRepository _userRepository=userRepository;
        private readonly IMapper _mapper=mapper;

        private async Task<bool> CheckIfUserExists(string email)
        {
           return await _userRepository.CheckIfUserExists(email);
        }

        public async Task CreateAsync(UserDto userDto)
        {
            var userExists=await CheckIfUserExists(userDto.Email);
            if(userExists)
            {
                throw new Exception();
            }
           
            var user=_mapper.Map<User>(userDto);
            await _userRepository.CreateAsync(user);
        }

        public async Task DeleteAsync(Guid userId)
        {
            await _userRepository.DeleteAsync(userId);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
           var users = await _userRepository.GetAllAsync();
           var usersDto=new List<UserDto>();
            foreach (var user in users)
            {
                usersDto.Add(new UserDto()
                { 

                    Address = user.Address,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                });
            }
            return usersDto;
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user != null)
            {
                var userDto=new UserDto()
                {
                    Address = user.Address,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,

                };
                return userDto;
            }
            return null;
        }
        public async Task UpdateAsync(Guid id,UserDto userDto)
        {
            var user = new User()
            {
                UserId = id,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Address = userDto.Address,
                PhoneNumber = userDto.PhoneNumber,
            };
            await _userRepository.UpdateAsync(user);
        }
    }
}
