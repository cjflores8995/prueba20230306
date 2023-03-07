using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Aplicacion.Logica.Movimientos;

using Dominio.Modelos;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    // https://server:<PORT>/api/movimiento
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientosController : MiControllerBase
    {
        [HttpPost("{id}")]
        public async Task<ActionResult<Unit>> Crear(Guid id, Nuevo.Ejecuta data)
        {
            data.CuentaId = id;
            return await Mediator.Send(data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<MovimientoDto>>> ObtenerPorCuenta(Guid id)
        {
            return await Mediator.Send(new ConsultarPorCuenta.ListaMovimientos { Id = id });
        }
    }
}
