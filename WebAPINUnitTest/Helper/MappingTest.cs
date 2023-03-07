using Aplicacion.Logica.Cuentas;

using AutoMapper;

using Dominio.Modelos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Helper
{
    public class MappingTest: Profile
    {
        public MappingTest()
        {
            CreateMap<Cuenta, CuentaDto>();
        }
    }
}
