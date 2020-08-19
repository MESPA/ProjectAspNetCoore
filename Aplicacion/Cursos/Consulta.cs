using System.Collections.Generic;
using MediatR;
using Dominio;
using System.Threading.Tasks;
using System.Threading;
using Persistencia;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Aplicacion.Cursos
{
    public class Consulta
    {
        public class ListaCursos : IRequest<List<CursoDTO>> { }
        public class Manejador : IRequestHandler<ListaCursos, List<CursoDTO>>
        {
            //injections dependency
            private readonly CursosDbContext _context;
            private readonly IMapper _mapper;
            public Manejador(CursosDbContext context, IMapper mapper)
            {
                _context = context;
                //injection de mapper 
                _mapper = mapper;
            }
            public async Task<List<CursoDTO>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                var cursos = await _context.Curso.Include(x => x.InstructoresLink)
                .ThenInclude(x => x.Instructor).ToListAsync();

                var cursosdto = _mapper.Map<List<Curso>, List<CursoDTO>>(cursos);

                return cursosdto;
            }
        }
    }
}