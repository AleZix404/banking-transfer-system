using banking_transfer_system.EF.Datas;
using banking_transfer_system.EF.Entities;
using banking_transfer_system.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace banking_transfer_system.Repository.Class
{
    public class UserRepository : IUserRepository
    {
        private readonly BankTransferContext _context;

        public UserRepository(BankTransferContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}