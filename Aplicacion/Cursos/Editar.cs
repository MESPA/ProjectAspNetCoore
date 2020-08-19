using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorErrores;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Editar
    {   //macheo con el controller
        public class Ejecuta : IRequest
        {
            public Guid CursoId { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public List<Guid> ListaInstructor { get; set; }
            public decimal? Precio { get; set; }
            public decimal? Promocion { get; set; }
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

                //actualizar el precio del curso
                var precioentidad = _context.Precio.Where(x => x.CursoId == editar.CursoId).FirstOrDefault();
                if (precioentidad != null)
                {
                    precioentidad.Promocion = request.Promocion ?? precioentidad.Promocion;
                    precioentidad.PrecioActual = request.Precio ?? precioentidad.PrecioActual;
                }
                else
                {
                    precioentidad = new Precio
                    {
                        PrecioId = Guid.NewGuid(),
                        PrecioActual = request.Precio ?? 0,
                        Promocion = request.Promocion ?? 0,
                        CursoId = editar.CursoId
                    };
                    await _context.Precio.AddAsync(precioentidad);
                }
                ///fin


                if (request.ListaInstructor != null)
                {
                    if (request.ListaInstructor.Count > 0)
                    {
                        //Eliminar los instructores actuales de la base de 
                        var instructorDB = _context.CursoInstructor.Where(x => x.CursoId == request.CursoId);
                        foreach (var instructoreliminar in instructorDB)
                        {
                            _context.CursoInstructor.Remove(instructoreliminar);
                        }
                        //fin del procedimeiento para eliminar inst

                        //Agregar instructores que provienen del cliente
                        foreach (var id in request.ListaInstructor)
                        {
                            var nuevoinstructor = new CursoInstructor
                            {
                                CursoId = request.CursoId,
                                InstructorId = id,
                            }; _context.CursoInstructor.Add(nuevoinstructor);
                            //fin 
                        }
                    }

                }
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