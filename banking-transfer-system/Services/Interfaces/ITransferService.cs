using banking_transfer_system.EF.DTOs;
using banking_transfer_system.EF.Entities;

namespace banking_transfer_system.Services.Interfaces
{
    public interface ITransferService
    {
        Task CreateTransferAsync(AccountTransferRequestDto transferDto);
        Task<IEnumerable<TransferDTO>> GetAccountHistoryAsync(string accountNumber);
    }
}
