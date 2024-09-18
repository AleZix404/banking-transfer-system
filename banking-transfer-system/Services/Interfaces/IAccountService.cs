using banking_transfer_system.EF.Entities;

namespace banking_transfer_system.Services.Interfaces
{
    public interface IAccountService
    {
        public Task<Account> GetAccountStatusAsync(string accountNumber);
        public Task<Account> GetAccountByNumberAsync(string accountNumber);
        public Task UpdateAccountBalancesAsync(Account sourceAccount, Account destinationAccount, decimal amount);
    }
}
