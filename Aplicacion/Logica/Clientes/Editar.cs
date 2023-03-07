using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Aplicacion.ManejadorError;
using Aplicacion.Mensajes;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Persistence;

namespace Aplicacion.Logica.Clientes
{
    public class Editar
    {
        public class Ejecuta : IRequest
        {
            public Guid ClienteId { get; set; }
            public string Nombres { get; set; }
            public string Genero { get; set; }
            public int Edad { get; set; }
            public string Identificacion { get; set; }
            public string Direccion { get; set; }
            public string Telefono { get; set; }
            public string Password { get; set; }
            public bool Estado { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Nombres).NotEmpty().Length(5, 120);
                RuleFor(x => x.Genero).NotEmpty().Length(8, 9);
                RuleFor(x => x.Edad).NotEmpty().ExclusiveBetween(0, 70);
                //RuleFor(x => x.Identificacion).NotEmpty().Length(10, 13);
                RuleFor(x => x.Direccion).NotEmpty().Length(10, 250);
                RuleFor(x => x.Telefono).NotEmpty().Length(9, 15);
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.Estado).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly BcoDbContext _context;
            public Manejador(BcoDbContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var cliente = await _context.Cliente.FirstOrDefaultAsync(x => x.ClienteId == request.ClienteId);
                if (cliente == null)
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = MSG_EX.CLIENTE_NO_ENCONTRADO });

                var persona = await _context.Persona.FindAsync(cliente.PersonaId);

                if (persona == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = MSG_EX.PERSONA_NO_ENCONTRADA });
                }
                //Update persona
                persona.Nombres = request.Nombres ?? persona.Nombres;
                persona.Genero = request.Genero ?? persona.Genero;
                persona.Edad = request.Edad;
                persona.Direccion = request.Direccion ?? persona.Direccion;
                persona.Telefono = request.Telefono ?? persona.Telefono;
                //_context.Persona.Update(persona);

                //Update Cliente
                cliente.Password = request.Password ?? cliente.Password;
                cliente.Estado = request.Estado;
                //_context.Cliente.Update(cliente);

                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                    return Unit.Value;

                throw new Exception(MSG_EX.INFORMACION_NO_EDITADA);
            }
        }
    }
}