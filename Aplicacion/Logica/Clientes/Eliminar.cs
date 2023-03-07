using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Aplicacion.ManejadorError;
using Aplicacion.Mensajes;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Persistence;

namespace Aplicacion.Logica.Clientes
{
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public Guid ClienteId { get; set; }
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
                // Validaciones de existencia de data
                var cliente = await _context.Cliente.FirstOrDefaultAsync(x => x.ClienteId == request.ClienteId);
                if (cliente == null)
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = MSG_EX.CLIENTE_NO_ENCONTRADO });

                var persona = await _context.Persona.FindAsync(cliente.PersonaId);
                if (persona == null)
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = MSG_EX.PERSONA_NO_ENCONTRADA });

                // Eliminar cuentas
                var cuentas = _context.Cuenta.Where(x => x.ClienteId == cliente.ClienteId).ToList();

                if (cuentas.Any())
                {
                    // Eliminar movimientos
                    foreach (var cuenta in cuentas)
                    {
                        // Eliminar movimientos
                        var movimientos = _context.Movimiento.Where(x => x.CuentaId == cuenta.CuentaId).ToList();

                        if (movimientos.Any())
                            _context.Movimiento.RemoveRange(movimientos);
                    }

                    _context.Cuenta.RemoveRange(cuentas);
                }

                // Eliminar cliente
                _context.Remove(cliente);

                // Eliminar persona
                _context.Remove(persona);

                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                    return Unit.Value;

                throw new Exception(MSG_EX.INFORMACION_NO_ELIMINADA);
            }
        }
    }
}