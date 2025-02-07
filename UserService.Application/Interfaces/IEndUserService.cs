using UserService.Application.DTOs;

namespace UserService.Application.Interfaces
{
    public interface IEndUserService
    {
        Task CreateAsync(UserDto userDto);
        Task<UserDto?> GetUserByIdAsync(Guid userId);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task UpdateAsync(Guid id,UserDto userDto);
        Task DeleteAsync(Guid userId);
        
    }
}
