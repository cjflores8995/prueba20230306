using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Aplicacion.ManejadorError;
using Aplicacion.Mensajes;

using MediatR;

using Persistence;

namespace Aplicacion.Logica.Cuentas
{
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public Guid CuentaId { get; set; }
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


                // Eliminar cuentas
                var cuenta = _context.Cuenta.Find(request.CuentaId);

                if (cuenta == null)
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = MSG_EX.CTA_NO_ENCONTRADA });

                // Eliminar Movimientos
                var movimientos = _context.Movimiento.Where(x => x.CuentaId == cuenta.CuentaId).ToList();

                if (movimientos.Any())
                    _context.Movimiento.RemoveRange(movimientos);

                _context.Cuenta.Remove(cuenta);
                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                    return Unit.Value;

                throw new Exception(MSG_EX.INFORMACION_NO_ELIMINADA);
            }
        }
    }
}