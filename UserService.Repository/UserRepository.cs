using UserService.Domain;
using UserService.Repository.Interfaces;

namespace UserService.Repository
{
    public class UserRepository : IUserRepository
    {
        public async Task<bool> CheckIfEmailExists(string email)
        {
            return false;
        }

        public async Task<bool> CheckIfPhoneNumberExists(string phoneNumber)
        {
            return false;
        }

        public Task CreateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(User user)
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
