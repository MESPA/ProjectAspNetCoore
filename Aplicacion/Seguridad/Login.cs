using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorErrores;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class Login
    {
        public class Ejecuta : IRequest<usuarioData>
        {
            public string Email { get; set; }
            public string Password { get; set; }

        }
        //validar login 
        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        //logica de los usuarios
        public class Manejador : IRequestHandler<Ejecuta, usuarioData>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly SignInManager<Usuario> _signInManager;
            public Manejador(UserManager<Usuario> userManager, SignInManager<Usuario> signManager)
            {
                _signInManager = signManager;
                _userManager = userManager;
            }

            public async Task<usuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByEmailAsync(request.Email);
                if (usuario == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
                }
                var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);
                if (resultado.Succeeded)
                {
                    return new usuarioData
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        Token = "Esta sera la data del ton",
                        UserName = usuario.UserName,
                        Email = usuario.Email,
                        Imagen = null



                    };
                }
                throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);


            }
        }
    }
}