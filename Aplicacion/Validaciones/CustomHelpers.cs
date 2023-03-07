using Aplicacion.ManejadorError;
using Aplicacion.Mensajes;

using Dominio.Enumeradores;
using Dominio.Modelos;

using Persistence;

using System;
using System.Linq;
using System.Net;

namespace Aplicacion.Validaciones
{
    public static class CustomHelpers
    {
        public const decimal limiteDiario = 1000;

        /// <summary>
        /// Valida que la transaccion sea consistente con el saldo de la cuenta y devuelve el tipo de transaccion
        /// </summary>
        /// <param name="Valor">Valor de la transaccion</param>
        /// <param name="SaldoInicial">Saldo incial de la cuenta</param>
        /// <param name="ultimoMovimiento">Ultimo movimiento en caso de existir</param>
        /// <returns></returns>
        /// <exception cref="ManejadorExcepcion"></exception>
        public static (string, decimal) ObtenerSaldoFinal(decimal Valor, decimal SaldoInicial, Movimiento ultimoMovimiento)
        {
            string _tipoMovimiento = string.Empty;
            decimal _saldoFinal = 0;

            if (ultimoMovimiento == null)
            {
                // Insertar el movimiento tomando el saldo de la cuenta
                if (Valor > 0)
                {
                    _saldoFinal = SaldoInicial + Valor;
                    _tipoMovimiento = TipoMovimiento.Credito.ToString();
                }
                else
                {
                    if (Math.Abs(Valor) > SaldoInicial)
                    {
                        throw new ManejadorExcepcion(HttpStatusCode.Unauthorized, new { mensaje = MSG_EX.SALDO_NO_DISPONIBLE });
                    }

                    _saldoFinal = SaldoInicial - Math.Abs(Valor);
                    _tipoMovimiento = TipoMovimiento.Debito.ToString();
                }

            }
            else
            {
                // Insertar el movimiento basado en el ultimo saldo de movimientos
                if (Valor > 0)
                {
                    _saldoFinal = ultimoMovimiento.Saldo + Valor;
                    _tipoMovimiento = TipoMovimiento.Credito.ToString();
                }
                else
                {
                    if (Math.Abs(Valor) > ultimoMovimiento.Saldo)
                    {
                        throw new ManejadorExcepcion(HttpStatusCode.Unauthorized, new { mensaje = MSG_EX.SALDO_NO_DISPONIBLE });
                    }

                    _saldoFinal = ultimoMovimiento.Saldo - Math.Abs(Valor);
                    _tipoMovimiento = TipoMovimiento.Debito.ToString();
                }
            }

            return (_tipoMovimiento, _saldoFinal);
        }

        /// <summary>
        /// Obtiene el valor final de los movimientos
        /// </summary>
        /// <param name="cuentaId">Numero de cuenta</param>
        /// <param name="_context">Contexto de la base de datos</param>
        /// <param name="fechaInicio">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha fin</param>
        /// <exception cref="ManejadorExcepcion"></exception>
        public static decimal ObtenerMovimientosValor(Guid CuentaId, BcoDbContext _context, DateTime fechaInicio, DateTime fechaFin)
        {
            decimal movimiento = 0;
            try
            {
                var listaMovimientos = _context.Movimiento.Where(
                x => x.CuentaId == CuentaId
                && x.Fecha.Date >= fechaInicio.Date
                && x.Fecha.Date <= fechaFin.Date);

                if (listaMovimientos.Any())
                {
                    decimal creditos = listaMovimientos.Where(x => x.TipoMovimiento == TipoMovimiento.Credito.ToString()).Sum(x => x.Valor);
                    decimal debitos = listaMovimientos.Where(x => x.TipoMovimiento == TipoMovimiento.Debito.ToString()).Sum(x => x.Valor);

                    movimiento = creditos - debitos;
                }

                return movimiento;
            }
            catch
            {
                return 0;
            }


        }
    }
}