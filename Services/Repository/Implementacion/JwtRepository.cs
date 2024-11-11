using API_MOV_ALM.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Repository.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository.Implementacion
{
    public class JwtRepository : IJwtRepository<Jwt>
    {
        public IConfiguration _configuration;
        public JwtRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        public string GenerarToken(Jwt jwtClass)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, jwtClass?.NombreUsuario?? ""),
               
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);
            string tokenCreado = tokenHandler.WriteToken(token);

            return tokenCreado;

        }
    }
}
