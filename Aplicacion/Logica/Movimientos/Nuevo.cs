using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Aplicacion.ManejadorError;
using Aplicacion.Mensajes;

using Dominio.Enumeradores;
using Dominio.Modelos;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Persistence;

namespace Aplicacion.Logica.Movimientos
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public Guid CuentaId { get; set; }
            public decimal Valor { get; set; }
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
                Guid _movimientoId = Guid.NewGuid();
                decimal _saldoFinal = 0;
                string _tipoMovimiento = string.Empty;

                var cuenta = await _context.Cuenta.FirstOrDefaultAsync(x => x.CuentaId == request.CuentaId);
                if (cuenta == null)
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = MSG_EX.CTA_NO_ENCONTRADA });

                //buscar ultimo movimiento existente
                var ultimoMovimiento = _context.Movimiento.Where(x => x.CuentaId == cuenta.CuentaId).OrderByDescending(x => x.Fecha).FirstOrDefault();

                (_tipoMovimiento, _saldoFinal) = Validaciones.CustomHelpers.ObtenerSaldoFinal(request.Valor, cuenta.SaldoInicial, ultimoMovimiento);

                var movimiento = new Movimiento
                {
                    CuentaId = request.CuentaId,
                    MovimientoId = _movimientoId,
                    Fecha = DateTime.UtcNow.AddHours(-5),
                    TipoMovimiento = _tipoMovimiento,
                    Valor = Math.Abs(request.Valor),
                    Saldo = _saldoFinal
                };

                _context.Movimiento.Add(movimiento);

                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                    return Unit.Value;

                throw new Exception(MSG_EX.INFORMACION_NO_AGREGADA);
            }
        }
    }
}