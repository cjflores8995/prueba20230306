using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Aplicacion.Logica.Cuentas;
using Aplicacion.Logica.Reportes;

using Dominio.Modelos;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    // https://server:<PORT>/api/reportes
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController : MiControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<List<ReporteDto>>> ObtenerPorCliente(Guid id, ConsultarPorCliente.ListadoReporte data)
        {
            data.ClienteId = id;
            return await Mediator.Send(data);
        }
    }
}
