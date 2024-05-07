using CRM.LIB.LO;
using CRM_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CRM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        private readonly GUsuarios _usuariosService;
        private readonly string secretKey = "GpBrkzSTI7Y2YdUUHVfGaMkKyUgDC0wGVaPuRnf4EossUOh";
        public AutenticacionController(GUsuarios usuariosService) 
        {
            _usuariosService = usuariosService;
        }

        [HttpPost]
        [Route("Autenticar")]
        public async Task<ActionResult<Object>> Autenticar([FromBody]AutenticacionRequest eUsuario)
        {
            var usuario = await _usuariosService.Autenticar(eUsuario.Cuenta, eUsuario.Contrasena);
            if (usuario != null)
            {
                var keyBayte = Encoding.ASCII.GetBytes(secretKey);
                var claim = new ClaimsIdentity();

                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.Name, eUsuario.Cuenta),
                };

                claim.AddClaims(claims);
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = claim,
                    Expires = DateTime.Now.AddHours(8),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(keyBayte),
                        SecurityAlgorithms.HmacSha256Signature
                        )
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
                string token = tokenHandler.WriteToken(tokenConfig);

                return StatusCode(StatusCodes.Status200OK,
                    new
                    {
                        Token = token,
                        IdUsuario = usuario.Id,
                        Usuario = $"{usuario.Nombre} {usuario.Apellidos}",
                        Cargo = usuario.Cargo
                    });
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });
            }
        }
    }
}
