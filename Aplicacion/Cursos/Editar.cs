using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorErrores;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Editar
    {   //macheo con el controller
        public class Ejecuta : IRequest
        {
            public int CursoId { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
        }
        //validacion con fluent
        public class EjecutaValidar : AbstractValidator<Ejecuta>
        {
            public EjecutaValidar()
            {
                RuleFor(c => c.Titulo).NotEmpty();
                RuleFor(c => c.Descripcion).NotEmpty();
                RuleFor(c => c.FechaPublicacion).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosDbContext _context;
            public Manejador(CursosDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var editar = await _context.Curso.FindAsync(request.CursoId);
                if (editar == null)
                {
                    //throw new Exception("El Curso no Existe"); 
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { curso = "No se encontro el curso" });

                }

                editar.Titulo = request.Titulo ?? editar.Titulo;
                editar.Descripcion = request.Descripcion ?? editar.Titulo;
                editar.FechaPublicacion = request.FechaPublicacion ?? editar.FechaPublicacion;

                var valor = await _context.SaveChangesAsync();



                if (valor > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se Edito el Curso");
            }
        }

    }
}