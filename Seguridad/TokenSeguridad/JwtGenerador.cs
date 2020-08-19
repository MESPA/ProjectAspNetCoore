using System.Collections.Generic;
using System.Security.Claims;
using Aplicacion.Contratos;
using Dominio;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;

namespace Seguridad
{
    //instalar desde el packamanage system.identityModel.tokens.jwt
    public class JwtGenerador : IJwtGenerador
    {

        public string CrearToken(Usuario usuario)
        {
            //crear ñogica de token
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, usuario.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescripcion = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = credenciales

            };

            var tokenmanejador = new JwtSecurityTokenHandler();
            var token = tokenmanejador.CreateToken(tokenDescripcion);

            return tokenmanejador.WriteToken(token);
        }


    }
}