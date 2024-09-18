using banking_transfer_system.EF.DTOs;
using banking_transfer_system.EF.Entities;
using banking_transfer_system.Repository.Interfaces;
using banking_transfer_system.Services.Interfaces;
using banking_transfer_system.SingleClass;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

namespace banking_transfer_system.Services.Class
{
    public class TransferService : ITransferService
    {
        private readonly IAccountService _accountService;
        private readonly ITransferRepository _transferRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TransferService(IAccountService accountService, ITransferRepository transferRepository, IUnitOfWork unitOfWork)
        {
            _accountService = accountService;
            _transferRepository = transferRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateTransferAsync(AccountTransferRequestDto transferDto)
        {
            try
            {
                var sourceAccount = await _accountService.GetAccountByNumberAsync(transferDto.SourceAccountNumber);
                var destinationAccount = await _accountService.GetAccountByNumberAsync(transferDto.DestinationAccountNumber);

                await _accountService.UpdateAccountBalancesAsync(sourceAccount, destinationAccount, transferDto.Amount);

                var transferData = new TransferData
                {
                    TransferRequest = transferDto,
                    SourceAccount = sourceAccount,
                    DestinationAccount = destinationAccount
                };

                await _transferRepository.RegisterTransfer(transferData);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Obtener más detalles de la excepción interna
                var innerException = ex.InnerException?.Message;
                Console.WriteLine($"Ocurrió un error al guardar los cambios: {innerException}");
                throw new Exception($"Ocurrió un error inesperado: {ex.Message} - {innerException}");
            }
        }

        async Task<IEnumerable<TransferDTO>> ITransferService.GetAccountHistoryAsync(string accountNumber)
        {
            return await _transferRepository.GetTransfersByAccountNumberAsync(accountNumber);
        }
    }

}
