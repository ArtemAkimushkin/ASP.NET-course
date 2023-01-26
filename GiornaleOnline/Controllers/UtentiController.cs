using GiornaleOnline.DataContext;
using GiornaleOnline.DataContext.Models;
using GiornaleOnline.Extensions;
using GiornaleOnline.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GiornaleOnline.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UtentiController : ControllerBase
    {
        private readonly ILogger<UtentiController> _logger;
        private readonly GOContext _dc;
        private readonly IConfiguration _config;
        public UtentiController(ILogger<UtentiController> logger, GOContext dc, IConfiguration config)
        {
            _logger = logger;
            _dc = dc;
            _config = config;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UtenteInfo>> Login(LoginDTO item)
        {
            var utente = await _dc.Utenti.FirstOrDefaultAsync(u => u.Username == item.Username && u.Password == item.Password);
            if (utente != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _config["Jwt:Subject"]!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("UserId", utente.Id.ToString()),
                    new Claim("DisplayName", utente.Nome!),
                    new Claim("Username", utente.Username!)
                };

                if (utente.Username == "Admin")
                {
                    claims.Add(new Claim(ClaimTypes.Role, "admin"));
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, "user"));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _config["Jwt:Issuer"]!,
                    _config["Jwt:Audience"]!,
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(_config.GetValue<double>("Jwt:ExpiresInMinutes")),
                    signingCredentials: signIn
                    );
                var uf = new UtenteInfo
                {
                    Utente = utente.ToUtenteModel(),
                    Token = new JwtSecurityTokenHandler().WriteToken(token)
                };
                return uf;
            }
            else
            {
                return BadRequest("Credenziali non valide");
            }
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UtenteModel>> Register(RegisterDTO item)
        {
            var utente = new Utente
            {
                Nome = item.Nome,
                Username = item.Username,
                Password = item.Password 
            };

            try
            {
                _dc.Utenti.Add(utente);
                await _dc.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException?.Message);
                return Problem(ex.InnerException?.Message);
            }

            return CreatedAtAction(nameof(Register), new {id = utente.Id }, utente.ToUtenteModel());
        }
    }
}
