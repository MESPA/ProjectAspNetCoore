using System.Linq;
using Aplicacion.Cursos;
using AutoMapper;
using Dominio;

namespace Aplicacion
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Curso, CursoDTO>()
            .ForMember(x => x.Instructor, y => y.MapFrom(z => z.InstructoresLink
             .Select(a => a.Instructor).ToList()));//mapeo manual
            CreateMap<CursoInstructor, CursoInstructorDTO>();
            CreateMap<Instructor, InstructorDTO>();
        }
    }
}