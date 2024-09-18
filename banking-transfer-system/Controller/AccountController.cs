using banking_transfer_system.EF.DTOs;
using banking_transfer_system.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace banking_transfer_system.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ITransferService _transferService;
        private readonly IAccountService _accountService;

        public AccountController(ITransferService transferService, IAccountService accountService)
        {
            _transferService = transferService;
            _accountService = accountService;
        }

        [HttpPost("createTransfer")]
        public async Task<IActionResult> CreateTransfer([FromBody] AccountTransferRequestDto transferDto)
        {
            try
            {
                await _transferService.CreateTransferAsync(transferDto);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error inesperado: {ex.Message}");
            }
        }

        [HttpGet("history/{accountNumber}")]
        public async Task<IActionResult> GetAccountHistory(string accountNumber)
        {
            try
            {
                var history = await _transferService.GetAccountHistoryAsync(accountNumber);

                if (history == null || !history.Any())
                {
                    return NotFound("No se encontraron transferencias para el numero de cuenta proporcionado.");
                }

                return Ok(history);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error inesperado: {ex.Message}");
            }
        }


        [HttpGet("status/{accountNumber}")]
        public async Task<IActionResult> GetAccountStatus(string accountNumber)
        {
            var account = await _accountService.GetAccountStatusAsync(accountNumber);

            try
            {
                if (account == null)
                    return NotFound("La cuenta no existe.");

                var accountDto = new AccountDto
                {
                    AccountNumber = account.AccountNumber,
                    Balance = account.Balance
                };

                return Ok(accountDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error inesperado: {ex.Message}");
            }
            
        }
    }
}
