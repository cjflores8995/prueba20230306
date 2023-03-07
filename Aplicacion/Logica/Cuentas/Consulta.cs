using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using Dominio.Modelos;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Persistence;

namespace Aplicacion.Logica.Cuentas
{
    public class Consulta
    {
        public class ListaCuentas : IRequest<List<CuentaDto>> { }

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
                var cuentas = await _context.Cuenta.ToListAsync();

                var cuentasDto = _mapper.Map<List<Cuenta>, List<CuentaDto>>(cuentas);

                return cuentasDto;
            }
        }


    }
}