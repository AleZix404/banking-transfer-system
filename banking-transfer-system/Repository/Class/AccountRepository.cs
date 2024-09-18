using banking_transfer_system.EF.Datas;
using banking_transfer_system.EF.Entities;
using banking_transfer_system.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace banking_transfer_system.Repository.Class
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BankTransferContext _context;

        public AccountRepository(BankTransferContext context)
        {
            _context = context;
        }

        public async Task<Account> GetAccountByNumberAsync(string accountNumber)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
        }
    }
}
