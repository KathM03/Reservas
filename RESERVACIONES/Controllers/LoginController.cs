using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RESERVACIONES.DTO;
using RESERVACIONES.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RESERVACIONES.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ReservasDbContext _context;
        private readonly IConfiguration _configuration;

        public LoginController(ReservasDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Datos ingresados no válidos.");
            }

            var usuario = await _context.Usuarios
                .Where(u => u.Email == loginDto.Email)
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                return Unauthorized("Credenciales Incorrectas.");
            }

            var passwordHasher = new PasswordHasher<Usuario>();
            var result = passwordHasher.VerifyHashedPassword(usuario, usuario.Contraseña, loginDto.Contraseña);

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Credenciales Incorrectas.");
            }

            var rol = await _context.Usuarios.Where(u => u.Email == loginDto.Email).Select(u => u.Rol).FirstOrDefaultAsync();

            var claims = new List<Claim>
                {
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim("FullName", $"{usuario.Nombre} {usuario.Apellido}"),
                new Claim("JoinDate", DateTime.Now.ToString("yyyy-MM-dd")),
                new Claim(ClaimTypes.Role, rol)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}

