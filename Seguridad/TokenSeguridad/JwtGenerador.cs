using System.Collections.Generic;
using System.Security.Claims;
using Aplicacion.Contratos;
using Dominio;

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
                new Claim(JwtRegisteredClaimName)
            }
        }
    }
}