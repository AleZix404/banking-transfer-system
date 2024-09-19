using banking_transfer_system.EF.DTOs;
using banking_transfer_system.EF.Entities;
using banking_transfer_system.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace banking_transfer_system.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        [SwaggerOperation(Summary = "Registra un nuevo usuario", Description = "Recibe los datos de un usuario (nombre de usuario y contraseña) y lo registra en el sistema.")]
        [SwaggerResponse(200, "Usuario registrado exitosamente")]
        [SwaggerResponse(400, "El nombre de usuario ya está en uso")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                Log.Information("Intentando registrar un nuevo usuario con nombre {Username}", registerDto.Username);

                var existingUser = await _userService.GetUserByUsernameAsync(registerDto.Username);
                if (existingUser != null)
                {
                    Log.Warning("El nombre de usuario {Username} ya esta en uso", registerDto.Username);
                    return BadRequest("El nombre de usuario ya esta en uso.");
                }

                var passwordHash = _userService.HashPassword(registerDto.Password);

                var user = new User
                {
                    Username = registerDto.Username,
                    PasswordHash = passwordHash
                };

                await _userService.CreateUserAsync(user);

                Log.Information("Usuario {Username} registrado exitosamente", registerDto.Username);
                return Ok("Usuario registrado exitosamente.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al intentar registrar el usuario {Username}", registerDto.Username);
                return StatusCode(500, $"Ocurrio un error inesperado: {ex.Message}");
            }
        }

        [HttpPost("login")]
        [SwaggerOperation(Summary = "Autentica a un usuario y genera un token JWT", Description = "Recibe el nombre de usuario y la contraseña. Si las credenciales son correctas, genera un token JWT.")]
        [SwaggerResponse(200, "Autenticación exitosa, devuelve el token JWT", typeof(string))]
        [SwaggerResponse(401, "Usuario o contraseña incorrectos")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                Log.Information("Intentando autenticar al usuario {Username}", loginDto.Username);

                var user = await _userService.GetUserByUsernameAsync(loginDto.Username);

                if (user == null)
                {
                    Log.Warning("El usuario {Username} no existe", loginDto.Username);
                    return Unauthorized("El usuario no existe.");
                }

                if (!_userService.VerifyPassword(loginDto.Password, user.PasswordHash))
                {
                    Log.Warning("Contraseña incorrecta para el usuario {Username}", loginDto.Username);
                    return Unauthorized("La contraseña es incorrecta.");
                }

                var token = GenerateJwtToken(user);
                Log.Information("Autenticación exitosa para el usuario {Username}", loginDto.Username);

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al autenticar al usuario {Username}", loginDto.Username);
                return StatusCode(500, $"Ocurrio un error inesperado: {ex.Message}");
            }
        }

        private string GenerateJwtToken(User user)
        {
            Log.Information("Generando token JWT para el usuario {Username}", user.Username);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            Log.Information("Token JWT generado exitosamente para el usuario {Username}", user.Username);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
