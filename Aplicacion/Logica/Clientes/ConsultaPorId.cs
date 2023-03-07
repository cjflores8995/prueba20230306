using System;
using System.Threading;
using System.Threading.Tasks;

using Aplicacion.Logica.Personas;

using AutoMapper;

using Dominio.Modelos;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Persistence;

namespace Aplicacion.Logica.Clientes
{
    public class ConsultaPorId
    {
        public class Consulta : IRequest<PersonaDto>
        {
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<Consulta, PersonaDto>
        {
            private readonly BcoDbContext _context;
            private readonly IMapper _mapper;
            public Manejador(BcoDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<PersonaDto> Handle(Consulta request, CancellationToken cancellationToken)
            {
                var cliente = await _context.Persona.Include(x => x.ClienteUnico).FirstOrDefaultAsync(z => z.ClienteUnico.ClienteId == request.Id);

                var clienteDto = _mapper.Map<Persona, PersonaDto>(cliente);

                return clienteDto;
            }
        }
    }
}