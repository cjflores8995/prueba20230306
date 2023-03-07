using System;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using Dominio.Modelos;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Persistence;

namespace Aplicacion.Logica.Cuentas
{
    public class ConsultaPorId
    {
        public class Consulta : IRequest<CuentaDto>
        {
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<Consulta, CuentaDto>
        {
            private readonly BcoDbContext _context;
            private readonly IMapper _mapper;
            public Manejador(BcoDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<CuentaDto> Handle(Consulta request, CancellationToken cancellationToken)
            {
                var cuenta = await _context.Cuenta.FindAsync(request.Id);

                var cuentaDto = _mapper.Map<Cuenta, CuentaDto>(cuenta);

                return cuentaDto;
            }
        }
    }
}