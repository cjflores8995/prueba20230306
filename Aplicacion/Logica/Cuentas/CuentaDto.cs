using System;

namespace Aplicacion.Logica.Cuentas
{
    public class CuentaDto
    {
        public Guid ClienteId { get; set; }
        public Guid CuentaId { get; set; }
        public string NumeroCuenta { get; set; }
        public string TipoCuenta { get; set; }
        public decimal SaldoInicial { get; set; }
        public bool Estado { get; set; }
    }
}