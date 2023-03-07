using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using Dominio.Modelos;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Persistence;

namespace Aplicacion.Logica.Cuentas
{
    // Devuelve unicamente el listado de cuentas del cliente
    public class ConsultaPorCliente
    {
        public class ListaCuentas : IRequest<List<CuentaDto>>
        {
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<ListaCuentas, List<CuentaDto>>
        {
            private readonly BcoDbContext _context;
            private readonly IMapper _mapper;
            public Manejador(BcoDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<CuentaDto>> Handle(ListaCuentas request, CancellationToken cancellationToken)
            {
                var cuentas = await _context.Cuenta.Where(x => x.ClienteId == request.Id).ToListAsync();

                var cuentasDto = _mapper.Map<List<Cuenta>, List<CuentaDto>>(cuentas);

                return cuentasDto;
            }
        }
    }
}