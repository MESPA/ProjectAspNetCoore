using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorErrores;
using AutoMapper;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class ConsultaId
    {
        public class CursoUnico : IRequest<CursoDTO>
        {
            public Guid Id { get; set; }
        }
        public class Manejador : IRequestHandler<CursoUnico, CursoDTO>
        {
            private readonly CursosDbContext _context;
            private readonly IMapper _mapper;
            public Manejador(CursosDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<CursoDTO> Handle(CursoUnico request, CancellationToken cancellationToken)
            {//buscar en la tabla cusros e instructor 
                var curso = await _context.Curso
                .Include(x => x.PrecioPromocion)
                .Include(x => x.ComentarioLista)
                .Include(x => x.InstructoresLink)
                .ThenInclude(y => y.Instructor)
                .FirstOrDefaultAsync(a => a.CursoId == request.Id);

                if (curso == null)
                {
                    //throw new Exception("El Curso no Existe"); 
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { curso = "No se encontro el curso" });

                }
                var cursodto = _mapper.Map<Curso, CursoDTO>(curso);
                return cursodto;
            }
        }
    }
}