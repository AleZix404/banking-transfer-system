using banking_transfer_system.EF.DTOs;
using banking_transfer_system.Services.Interfaces;
using banking_transfer_system.SingleClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

        [Authorize]
        [HttpPost("createTransfer")]
        [SwaggerOperation(Summary = "Crea una transferencia entre cuentas", Description = "Recibe los números de cuenta de origen y destino junto con el monto a transferir.")]
        [SwaggerResponse(200, "Transferencia creada exitosamente")]
        [SwaggerResponse(400, "Error en la solicitud: cuenta no encontrada o saldo insuficiente")]
        [SwaggerResponse(500, "Error inesperado")]
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

        [Authorize]
        [HttpGet("history/{accountNumber}")]
        [SwaggerOperation(Summary = "Obtiene el historial de transferencias de una cuenta", Description = "Proporciona el historial de todas las transferencias realizadas o recibidas por la cuenta especificada.")]
        [SwaggerResponse(200, "Historial encontrado", typeof(IEnumerable<TransferDTO>))]
        [SwaggerResponse(404, "No se encontraron transferencias para el número de cuenta proporcionado")]
        [SwaggerResponse(500, "Error inesperado")]
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

        [Authorize]
        [HttpGet("status/{accountNumber}")]
        [SwaggerOperation(Summary = "Obtiene el estado de una cuenta", Description = "Devuelve el saldo actual de la cuenta especificada.")]
        [SwaggerResponse(200, "Estado de cuenta encontrado", typeof(AccountDto))]
        [SwaggerResponse(404, "La cuenta no existe")]
        [SwaggerResponse(500, "Error inesperado")]
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
