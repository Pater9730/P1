using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using P1.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace P1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtController : ControllerBase
    {
        private readonly IConfiguration _config;

        public JwtController (IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public ActionResult login([FromBody] Usuario login)
        {
            if (login.Correo == "Admin" && login.Clave == "1234")
            {
                return Unauthorized();
            }
            var Token = generartoken(login.Correo);
            return Ok(new { Token });
        }
        private string generartoken(string Correo)
        {
            var JwtSettings = _config.GetSection("Jwt");
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings["Key"]!));
            var cred = new SigningCredentials(Key,SecurityAlgorithms.HmacSha256);

            var Claims = new[]
            {
                new Claim(ClaimTypes.Name,Correo),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var Token = new JwtSecurityToken(
                issuer: JwtSettings["Issuer"],
                audience: JwtSettings["Audience"],
                claims: Claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: cred);

            return new JwtSecurityTokenHandler().WriteToken(Token);
                

        }
    }
}
