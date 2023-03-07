using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Aplicacion.ManejadorError;
using Aplicacion.Mensajes;

using Dominio.Modelos;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Persistence;

namespace Aplicacion.Logica.Cuentas
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public Guid ClienteId { get; set; }
            public string NumeroCuenta { get; set; }
            public string TipoCuenta { get; set; }
            public decimal SaldoInicial { get; set; }
            public bool Estado { get; set; }

        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.NumeroCuenta).NotEmpty().Length(5, 15);
                RuleFor(x => x.TipoCuenta).NotEmpty().Length(5, 15);
            }
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
                Guid _cuentaId = Guid.NewGuid();

                var CuentaExiste = await _context.Cuenta.FirstOrDefaultAsync(x => x.NumeroCuenta == request.NumeroCuenta);
                if (CuentaExiste != null)
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = MSG_EX.CTA_YA_EXISTE });

                var cliente = await _context.Cliente.FirstOrDefaultAsync(x => x.ClienteId == request.ClienteId);
                if (cliente == null)
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = MSG_EX.CLIENTE_NO_ENCONTRADO });

                if (request.SaldoInicial < 0)
                    throw new ManejadorExcepcion(HttpStatusCode.Unauthorized, new { mensaje = MSG_EX.SALDO_INCONSISTENTE });

                var cuenta = new Cuenta
                {
                    ClienteId = request.ClienteId,
                    CuentaId = _cuentaId,
                    NumeroCuenta = request.NumeroCuenta,
                    TipoCuenta = request.TipoCuenta,
                    SaldoInicial = request.SaldoInicial,
                    Estado = true
                };

                _context.Cuenta.Add(cuenta);

                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                    return Unit.Value;

                throw new Exception("No se pudo crear la cuenta");
            }
        }
    }
}