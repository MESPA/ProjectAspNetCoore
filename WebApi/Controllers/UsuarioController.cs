using System.Threading.Tasks;
using Aplicacion.Seguridad;
using Dominio;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class UsuarioController : MiControllerBase
    {
        //http://localhost:5000/api/Usuario/login
        [HttpPost("login")]
        public async Task<ActionResult<usuarioData>> Login(Login.Ejecuta parametros)
        {
            return await Mediator.Send(parametros);
        }
    }
}