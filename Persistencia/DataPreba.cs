using System.Linq;
using System.Threading.Tasks;
using Dominio;
using Microsoft.AspNetCore.Identity;

namespace Persistencia
{
    public class DataPreba
    {
        public static async Task Insertdata(CursosDbContext context, UserManager<Usuario> usuarioManager)
        {
            if (!usuarioManager.Users.Any())
            {
                var usuario = new Usuario { NombreCompleto = "manuel", UserName = "manespa", Email = "manuel@gmail.com" };
                await usuarioManager.CreateAsync(usuario, "Emy.ez30");
            }
        }
    }
}