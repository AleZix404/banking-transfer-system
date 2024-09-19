using banking_transfer_system.EF.Entities;

namespace banking_transfer_system.Services
{
    public interface IUserService
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task CreateUserAsync(User user);
        public string HashPassword(string password);
        public bool VerifyPassword(string password, string hashedPassword);
    }
}
