using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Aplicacion.ManejadorError;
using Aplicacion.Mensajes;

using AutoMapper;

using Dominio.Modelos;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Persistence;

namespace Aplicacion.Logica.Movimientos
{
    public class ConsultarPorCuenta
    {
        public class ListaMovimientos : IRequest<List<MovimientoDto>>
        {
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<ListaMovimientos, List<MovimientoDto>>
        {
            private readonly BcoDbContext _context;
            private readonly IMapper _mapper;
            public Manejador(BcoDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<MovimientoDto>> Handle(ListaMovimientos request, CancellationToken cancellationToken)
            {
                var cuenta = await _context.Cuenta.FirstOrDefaultAsync(x => x.CuentaId == request.Id);
                if (cuenta == null)
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = MSG_EX.CTA_NO_ENCONTRADA });

                var movimientos = await _context.Movimiento.Where(x => x.CuentaId == request.Id).OrderByDescending(x => x.Fecha).ToListAsync();

                var movimientosDto = _mapper.Map<List<Movimiento>, List<MovimientoDto>>(movimientos);

                return movimientosDto;
            }
        }
    }
}