using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Modelos
{
    [Index(nameof(Identificacion), IsUnique = true)]
    public class Persona
    {
        public Guid PersonaId { get; set; }

        [Required]
        [StringLength(120, MinimumLength =5)]
        public string Nombres { get; set; }

        [Required]
        [StringLength(9, MinimumLength = 8)]
        public string Genero { get; set; }

        public int Edad { get; set; }

        [Required]
        [StringLength(13, MinimumLength = 10)]
        public string Identificacion { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 10)]
        public string Direccion { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 10)]
        public string Telefono { get; set; }
        public Cliente ClienteUnico { get; set; }
    }
}
