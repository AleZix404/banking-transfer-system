using banking_transfer_system.EF.DTOs;
using banking_transfer_system.Services.Interfaces;
using banking_transfer_system.SingleClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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
                Log.Information("Intentando crear una transferencia entre la cuenta {SourceAccount} y {DestinationAccount} por un monto de {Amount}",
                    transferDto.SourceAccountNumber, transferDto.DestinationAccountNumber, transferDto.Amount);

                await _transferService.CreateTransferAsync(transferDto);
                Log.Information("Transferencia creada exitosamente entre {SourceAccount} y {DestinationAccount}",
                    transferDto.SourceAccountNumber, transferDto.DestinationAccountNumber);

                return Ok();
            }
            catch (ArgumentException ex)
            {
                Log.Warning("Error en la creación de la transferencia: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ocurrio un error inesperado al intentar crear la transferencia");
                return StatusCode(500, $"Ocurrió un error inesperado: {ex.Message}");
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
                Log.Information("Consultando el historial de transferencias para la cuenta {AccountNumber}", accountNumber);

                var history = await _transferService.GetAccountHistoryAsync(accountNumber);

                if (history == null || !history.Any())
                {
                    Log.Warning("No se encontraron transferencias para la cuenta {AccountNumber}", accountNumber);
                    return NotFound("No se encontraron transferencias para el numero de cuenta proporcionado.");
                }

                Log.Information("Historial de transferencias obtenido para la cuenta {AccountNumber}", accountNumber);
                return Ok(history);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ocurrio un error inesperado al consultar el historial de transferencias para la cuenta {AccountNumber}", accountNumber);
                return StatusCode(500, $"Ocurrió un error inesperado: {ex.Message}");
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
            try
            {
                Log.Information("Consultando el estado de la cuenta {AccountNumber}", accountNumber);

                var account = await _accountService.GetAccountStatusAsync(accountNumber);

                if (account == null)
                {
                    Log.Warning("La cuenta {AccountNumber} no existe", accountNumber);
                    return NotFound("La cuenta no existe.");
                }

                var accountDto = new AccountDto
                {
                    AccountNumber = account.AccountNumber,
                    Balance = account.Balance
                };

                Log.Information("Estado de cuenta obtenido para la cuenta {AccountNumber}", accountNumber);
                return Ok(accountDto);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ocurrio un error inesperado al consultar el estado de la cuenta {AccountNumber}", accountNumber);
                return StatusCode(500, $"Ocurrio un error inesperado: {ex.Message}");
            }
        }
    }
}
