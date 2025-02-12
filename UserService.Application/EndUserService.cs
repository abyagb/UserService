using AutoMapper;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Domain;
using UserService.Repository.Interfaces;

namespace UserService.Application
{
    public class EndUserService(IUserRepository userRepository,IMapper mapper) : IEndUserService
    {
        //remove unecessary fields
        private readonly IUserRepository _userRepository=userRepository;
        private readonly IMapper _mapper=mapper;

        //all private methods should be at the bottom of the class
        private async Task<bool> CheckIfUserExists(string email) //rename this to be more specific, like CheckIfUserExistsByEmailAsync because we are only cvhecking email here
        {
            //what about the phone number? we should check if the phone number exists too
           return await _userRepository.CheckIfUserExists(email);
        }

        //for any new logic you write, always have tests for them...this is a good practice
        public async Task CreateAsync(UserDto userDto)
        {
            //this should be wrapped in a try catch block, and the exception should be caught and logged
            var userExists=await CheckIfUserExists(userDto.Email); //there needs to be a space between =
            if(userExists)
            {
                //research how to create custom exception because this exception is too generic
                //A custom exception called EntityExistsException should be created and thrown here with a meaningful error message
                throw new Exception(); //why are there no logs?
            }
           
            //there needs to be spaces between the = sign
            var user=_mapper.Map<User>(userDto);
            user.UserId=Guid.NewGuid();
            await _userRepository.CreateAsync(user);
        }

        public async Task DeleteAsync(Guid userId)
        {
            //no try catch? what if the user does not exist?
            //also, why are we not logging the deletion of the user?
            await _userRepository.DeleteAsync(userId);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            //no try catch? what if something goes wrong? 
           var users = await _userRepository.GetAllAsync();
           var usersDto=new List<UserDto>(); //space between =
            foreach (var user in users)
            {
                usersDto.Add(new UserDto
                { 
//why is this space here? there should be no space between the method and the opening bracket
                    Address = user.Address, //why is automapper not being used here
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
            //no try catch? what if the user does not exist?
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user != null)
            {
                var userDto=new UserDto() //why arent we using the mapper here? the purpose of the mapper is to map objects so that we dont have long lines of code like this
                {
                    Address = user.Address,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
//why is this space here
                };
                return userDto;
            }
            return null;
        } //there should be a space between the closing bracket and the method
        public async Task UpdateAsync(Guid id,UserDto userDto) //space between ,
        {
            //what if I only want to update the first name? we should only update the fields that are not null
            //research a better way to update the user
            var user = new User
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
