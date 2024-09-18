using banking_transfer_system.EF.Entities;

namespace banking_transfer_system.Repository.Interfaces
{
    public interface IAccountRepository
    {
        public Task<Account> GetAccountByNumberAsync(string accountNumber);
    }
}
