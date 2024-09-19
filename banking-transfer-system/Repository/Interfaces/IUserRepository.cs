using banking_transfer_system.EF.Entities;
using System.Threading.Tasks;

namespace banking_transfer_system.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task CreateUserAsync(User user);
    }
}