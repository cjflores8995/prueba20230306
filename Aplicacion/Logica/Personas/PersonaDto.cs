using Aplicacion.Logica.Clientes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Logica.Personas
{
    public class PersonaDto
    {
        public string Nombres { get; set; }
        public string Genero { get; set; }
        public int Edad { get; set; }
        public string Identificacion { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public ClienteDto Cliente { get; set; }
    }
}
