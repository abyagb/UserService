﻿using UserService.Domain;

namespace UserService.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task CreateAsync(User user);
        Task<User?> GetUserByIdAsync(Guid userId); 
        Task<IEnumerable<User>> GetAllAsync();
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<bool> CheckIfUserExists(string email);
    }
}
