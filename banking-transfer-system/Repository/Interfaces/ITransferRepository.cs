using banking_transfer_system.EF.DTOs;
using banking_transfer_system.EF.Entities;
using banking_transfer_system.SingleClass;

namespace banking_transfer_system.Repository.Interfaces
{
    public interface ITransferRepository
    {
        Task<IEnumerable<TransferDTO>> GetTransfersByAccountNumberAsync(string accountNumber);
        public Task AddAccountDataAsync(Transfer account);
        public Task RegisterTransfer(TransferData transferData);
    }
}
