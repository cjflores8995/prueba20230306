using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Aplicacion.ManejadorError;
using Aplicacion.Mensajes;

using MediatR;

using Persistence;

namespace Aplicacion.Logica.Cuentas
{
    public class Editar
    {
        public class Ejecuta : IRequest
        {
            public Guid CuentaId { get; set; }
            public bool Estado { get; set; }
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
                var cuenta = await _context.Cuenta.FindAsync(request.CuentaId);
                if (cuenta == null)
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = MSG_EX.CTA_NO_ENCONTRADA });

                //Update cuenta
                cuenta.Estado = request.Estado;

                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                    return Unit.Value;

                throw new Exception(MSG_EX.INFORMACION_NO_EDITADA);
            }
        }
    }
}