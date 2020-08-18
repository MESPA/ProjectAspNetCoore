using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistencia;
using Dominio;
using Microsoft.AspNetCore.Identity;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {//se moddifico este metodo para ejecutar las migra ciones con el comando dotnet watch run
            var hostserver = CreateHostBuilder(args).Build();
            using (var ambiente = hostserver.Services.CreateScope())
            {
                var services = ambiente.ServiceProvider;


                try
                {
                    var userrManager = services.GetRequiredService<UserManager<Usuario>>();
                    var context = services.GetRequiredService<CursosDbContext>();
                    context.Database.Migrate();
                    //registrar usuario en identity core
                    DataPreba.Insertdata(context, userrManager).Wait();

                }
                catch (Exception e)
                {
                    var Logging = services.GetRequiredService<ILogger<Program>>();
                    Logging.LogError(e, "Ocurrio un Error en la migracion");
                }
            }
            hostserver.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
