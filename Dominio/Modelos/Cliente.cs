using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Modelos
{
    public class Cliente
    {
        public Guid ClienteId { get; set; }
        public string Password { get; set; }
        public bool Estado { get; set; }
        public Guid PersonaId { get; set; }
        public Persona Persona { get; set; }
        ICollection<Cuenta> CuentasLista { get; set; }

    }
}
