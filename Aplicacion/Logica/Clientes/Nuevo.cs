using Aplicacion.ManejadorError;
using Aplicacion.Mensajes;

using Dominio.Modelos;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Persistence;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Logica.Clientes
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public string Nombres { get; set; }
            public string Genero { get; set; }
            public int Edad { get; set; }
            public string Identificacion { get; set; }
            public string Direccion { get; set; }
            public string Telefono { get; set; }
            public string Password { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Nombres).NotEmpty().Length(5, 120);
                RuleFor(x => x.Genero).NotEmpty().Length(8, 9);
                RuleFor(x => x.Edad).NotEmpty().ExclusiveBetween(0, 70);
                RuleFor(x => x.Identificacion).NotEmpty().Length(10, 13);
                RuleFor(x => x.Direccion).NotEmpty().Length(10, 250);
                RuleFor(x => x.Telefono).NotEmpty().Length(9, 15);
                RuleFor(x => x.Password).NotEmpty();
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
                var PersonaExiste = await _context.Persona.FirstOrDefaultAsync(x => x.Identificacion == request.Identificacion);
                if (PersonaExiste != null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotAcceptable, new { mensaje = MSG_EX.IDENTIFICACION_YA_EXISTE });
                }
                    

                Guid _personaId = Guid.NewGuid();
                Guid _clienteId = Guid.NewGuid();

                var persona = new Persona
                {
                    PersonaId = _personaId,
                    Nombres = request.Nombres,
                    Genero = request.Genero,
                    Edad = request.Edad,
                    Identificacion = request.Identificacion,
                    Direccion = request.Direccion,
                    Telefono = request.Telefono
                };
                _context.Persona.Add(persona);

                var cliente = new Cliente
                {
                    ClienteId = _clienteId,
                    PersonaId = _personaId,
                    Password = request.Password,
                    Estado = true
                };
                _context.Cliente.Add(cliente);

                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception(MSG_EX.INFORMACION_NO_AGREGADA);
            }
        }
    }
}
