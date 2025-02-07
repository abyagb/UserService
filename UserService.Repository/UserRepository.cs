using UserService.Domain;
using UserService.Repository.Interfaces;

namespace UserService.Repository
{
    public class UserRepository :IUserRepository
    {
        public Task<bool> CheckIfUserExists(string email)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetUserByIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
