using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Aplicacion.ManejadorError;
using Aplicacion.Mensajes;

using Dominio.Enumeradores;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Persistence;

namespace Aplicacion.Logica.Reportes
{
    public class ConsultarPorCliente
    {
        public class ListadoReporte : IRequest<List<ReporteDto>>
        {
            public Guid ClienteId { get; set; }
            public DateTime FechaInicio { get; set; }
            public DateTime FechaFin { get; set; }
        }

        public class Manejador : IRequestHandler<ListadoReporte, List<ReporteDto>>
        {
            private readonly BcoDbContext _context;
            public Manejador(BcoDbContext context)
            {
                _context = context;
            }

            public async Task<List<ReporteDto>> Handle(ListadoReporte request, CancellationToken cancellationToken)
            {
                ReporteDto reporteDto = new ReporteDto();
                List<ReporteDto> listaReporteDto = new List<ReporteDto>();
                decimal movimiento = 0;

                var cliente = await _context.Cliente.FirstOrDefaultAsync(x => x.ClienteId == request.ClienteId);
                if (cliente == null)
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = MSG_EX.CLIENTE_NO_ENCONTRADO });

                var cuentas = await _context.Cuenta.Where(x => x.ClienteId == request.ClienteId).ToListAsync();

                foreach (var cuenta in cuentas)
                {
                    //Obtener saldo Final
                    var ultimoMovimiento = _context.Movimiento.Where(x => x.CuentaId == cuenta.CuentaId).OrderByDescending(x => x.Fecha).FirstOrDefault();
                    var saldoDisponible = (ultimoMovimiento != null) ? ultimoMovimiento.Saldo : cuenta.SaldoInicial;

                    movimiento = Validaciones.CustomHelpers.ObtenerMovimientosValor(cuenta.CuentaId, _context, request.FechaInicio, request.FechaFin);

                    reporteDto = new ReporteDto()
                    {
                        Fecha = request.FechaFin.Date,
                        NumeroCuenta = cuenta.NumeroCuenta,
                        Tipo = cuenta.TipoCuenta,
                        SaldoInicial = cuenta.SaldoInicial,
                        Estado = cuenta.Estado,
                        Movimiento = movimiento,
                        SaldoDisponible = saldoDisponible
                    };
                    listaReporteDto.Add(reporteDto);
                }

                return listaReporteDto;
            }
        }
    }
}
