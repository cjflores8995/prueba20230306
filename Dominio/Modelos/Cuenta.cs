using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Modelos
{
    [Index(nameof(NumeroCuenta), IsUnique = true)]
    public class Cuenta
    {
        public Guid CuentaId { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 5)]
        public string NumeroCuenta { get; set; }

        [StringLength(15, MinimumLength = 5)]
        [Required]
        public string TipoCuenta { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SaldoInicial { get; set; }

        public bool Estado { get; set; }

        public Guid ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public ICollection<Movimiento> MovimientosLista { get; set; }

    }
}
