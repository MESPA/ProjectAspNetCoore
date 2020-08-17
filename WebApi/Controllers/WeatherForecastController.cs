using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using Dominio;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        //inyeccion de dependencia usando el constructos de la clase traer data de la base de datos
        private readonly CursosDbContext context;
        public WeatherForecastController(CursosDbContext _context)
        {
            this.context = _context;

        }
        [HttpGet]

        public IEnumerable<Curso> Get()
        {

            return context.Curso.ToList();
        }


    }
}
