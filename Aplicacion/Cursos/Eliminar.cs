using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorErrores;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public Guid Id { get; set; }
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
                //eliminar instructor antes de eliminar el curso
                var instrDB = _context.CursoInstructor.Where(x => x.CursoId == request.Id);
                foreach (var Int in instrDB)
                {
                    _context.CursoInstructor.Remove(Int);
                }

                var comtDB = _context.Comentario.Where(x => x.CursoId == request.Id);
                foreach (var cmt in comtDB)
                {
                    _context.Comentario.Remove(cmt);
                }
                var precioDB = _context.Precio.Where(x => x.CursoId == request.Id).FirstOrDefault();
                if (precioDB != null)
                {
                    _context.Precio.Remove(precioDB);
                }


                var eliminar = await _context.Curso.FindAsync(request.Id);
                if (eliminar == null)
                {
                    //throw new Exception("No se Puede Eliminar el Curso"); 
                    // excepcion creada en la carpeta middelware
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { curso = "No se encontro el curso" });

                }

                _context.Remove(eliminar);
                var valor = await _context.SaveChangesAsync();
                if (valor > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se Pudieron Guardar los Cambios");

            }
        }
    }
}