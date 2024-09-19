using banking_transfer_system.EF.DTOs;
using banking_transfer_system.EF.Entities;
using banking_transfer_system.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        [SwaggerOperation(Summary = "Registra un nuevo usuario", Description = "Recibe los datos de un usuario (nombre de usuario y contraseña) y registra en el sistema.")]
        [SwaggerResponse(200, "Usuario registrado exitosamente")]
        [SwaggerResponse(400, "El nombre de usuario ya está en uso")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var existingUser = await _userService.GetUserByUsernameAsync(registerDto.Username);
            if (existingUser != null)
            {
                return BadRequest("El nombre de usuario ya esta en uso.");
            }

            var passwordHash = _userService.HashPassword(registerDto.Password);

            var user = new User
            {
                Username = registerDto.Username,
                PasswordHash = passwordHash
            };

            await _userService.CreateUserAsync(user);

            return Ok("Usuario registrado exitosamente.");
        }

        [HttpPost("login")]
        [SwaggerOperation(Summary = "Autentica a un usuario y genera un token JWT", Description = "Recibe el nombre de usuario y la contraseña. Si las credenciales son correctas, genera un token JWT.")]
        [SwaggerResponse(200, "Autenticación exitosa, devuelve el token JWT", typeof(string))]
        [SwaggerResponse(401, "Usuario o contraseña incorrectos")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userService.GetUserByUsernameAsync(loginDto.Username);

            if (user == null)
            {
                return Unauthorized("El usuario no existe.");
            }

            if (!_userService.VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                return Unauthorized("La contraseña es incorrecta.");
            }

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        private string GenerateJwtToken(User user)
        {
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

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}