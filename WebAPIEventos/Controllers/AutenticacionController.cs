using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIEventos.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using WebAPIEventos.Context;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Cors;

namespace WebAPIEventos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowFrontend")] 
    public class AutenticacionController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        public AutenticacionController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Object opData)
        {

            var data = JsonConvert.DeserializeObject<dynamic>(opData.ToString());
            if (string.IsNullOrEmpty(data?.correo?.ToString()) || string.IsNullOrEmpty(data?.password?.ToString()))
            {
                return BadRequest(new { Message = "El correo y la contraseña son obligatorios." });
            }
            string correo = data.correo.ToString();
            string password = data.password.ToString();
            var usuario = await _context.Usuarios
            .Where(u => u.Correo == correo && u.Password == password)
            .FirstOrDefaultAsync();

            if (usuario == null)
            {
                return NotFound(new { Message = "Datos incorrectos." });
            }
            var secretkey = _configuration.GetSection("settings").GetSection("secretkey").ToString();
            var keyBytes = Encoding.ASCII.GetBytes(secretkey);
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.Email, correo));
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
            string tokenCreado = tokenHandler.WriteToken(tokenConfig);
            return Ok(new {token =tokenCreado });
        }

    }
}
