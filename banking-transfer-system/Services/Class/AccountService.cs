using banking_transfer_system.EF.Entities;
using banking_transfer_system.Repository.Interfaces;
using banking_transfer_system.Services.Interfaces;

namespace banking_transfer_system.Services.Class
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Account> GetAccountByNumberAsync(string accountNumber)
        {
            return await _accountRepository.GetAccountByNumberAsync(accountNumber);
        }

        public async Task<Account> GetAccountStatusAsync(string accountNumber)
        {
            return await _accountRepository.GetAccountByNumberAsync(accountNumber);
        }
        public async Task UpdateAccountBalancesAsync(Account sourceAccount, Account destinationAccount, decimal amount)
        {

            if (sourceAccount == null)
                throw new ArgumentException("La cuenta de origen no existe.");

            if (destinationAccount == null)
                throw new ArgumentException("La cuenta de destino no existe.");

            if (sourceAccount.Balance < amount)
                throw new Exception("Saldo insuficiente en la cuenta de origen.");

            sourceAccount.Balance -= amount;
            destinationAccount.Balance += amount;
        }
    }
}
