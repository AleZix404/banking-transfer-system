using banking_transfer_system.EF.Datas;
using banking_transfer_system.EF.DTOs;
using banking_transfer_system.EF.Entities;
using banking_transfer_system.Repository.Interfaces;
using banking_transfer_system.SingleClass;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

namespace banking_transfer_system.Repository.Class
{
    public class TransferRepository : ITransferRepository
    {
        private readonly BankTransferContext _context;

        public TransferRepository(BankTransferContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TransferDTO>> GetTransfersByAccountNumberAsync(string accountNumber)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

            if (account == null)
            {
                throw new ArgumentException("La cuenta no existe.");
            }

            return await _context.Transfers
                .Where(t => t.SourceAccountId == account.Id || t.DestinationAccountId == account.Id)
                .Select(t => new TransferDTO
                {
                    SourceAccountId = t.SourceAccountId,
                    DestinationAccountId = t.DestinationAccountId,
                    Amount = t.Amount,
                    TransferDate = t.TransferDate
                })
                .ToListAsync();
        }

        public async Task RegisterTransfer(TransferData transferData)
        {
            var transfer = new Transfer
            {
                SourceAccountId = transferData.SourceAccount.Id,
                DestinationAccountId = transferData.DestinationAccount.Id,
                Amount = transferData.TransferRequest.Amount,
                TransferDate = DateTime.UtcNow
            };
            await AddAccountDataAsync(transfer);
        }
        public async Task AddAccountDataAsync(Transfer transfer)
        {
            await _context.Transfers.AddAsync(transfer);
        }
    }
}
