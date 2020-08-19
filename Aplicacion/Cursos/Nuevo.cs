using System;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using MediatR;
using Persistencia;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using System.Collections.Generic;

namespace Aplicacion.Cursos
{
    public class Nuevo
    {   //macheo con el controller
        public class Ejecuta : IRequest
        {

            //[Required(ErrorMessage = "Debe Ingresar un titulo")]
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public List<Guid> ListaInstructor { get; set; } //data de instructor
            public decimal Precio { get; set; }
            public decimal Promocion { get; set; }
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
                Guid _cursoId = Guid.NewGuid();
                var crear = new Curso
                {
                    CursoId = _cursoId,
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion
                };
                _context.Curso.Add(crear);
                if (request.ListaInstructor != null)
                {
                    foreach (var id in request.ListaInstructor)
                    {
                        var cursoInstructor = new CursoInstructor
                        {
                            CursoId = _cursoId,
                            InstructorId = id
                        };
                        _context.CursoInstructor.Add(cursoInstructor);
                    }
                }

                //logica para insertar un prcio al curso creado
                var precioentidad = new Precio
                {
                    CursoId = _cursoId,
                    PrecioActual = request.Precio,
                    Promocion = request.Promocion,
                    PrecioId = Guid.NewGuid()

                };
                _context.Precio.Add(precioentidad);

                var valor = await _context.SaveChangesAsync();
                if (valor > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se Inserto el Curso");

            }
        }
    }
}