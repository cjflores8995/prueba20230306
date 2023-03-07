using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Aplicacion.Logica.Cuentas;

using Dominio.Modelos;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    // https://server:<PORT>/api/cuenta
    [Route("api/[controller]")]
    [ApiController]
    public class CuentasController : MiControllerBase
    {
        [HttpPost("{id}")]
        public async Task<ActionResult<Unit>> Crear(Guid id, Nuevo.Ejecuta data)
        {
            data.ClienteId = id;
            return await Mediator.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Editar(Guid id, Editar.Ejecuta data)
        {
            data.CuentaId = id;
            return await Mediator.Send(data);
        }

        [HttpGet]
        public async Task<ActionResult<List<CuentaDto>>> Obtener()
        {
            return await Mediator.Send(new Consulta.ListaCuentas());
        }

        [HttpGet("Cliente/{id}")]
        public async Task<ActionResult<List<CuentaDto>>> ObtenerPorCliente(Guid id)
        {
            return await Mediator.Send(new ConsultaPorCliente.ListaCuentas { Id = id });
        }

        [HttpGet("Cuenta/{id}")]
        public async Task<ActionResult<CuentaDto>> ObtenerPorCuenta(Guid id)
        {
            return await Mediator.Send(new ConsultaPorId.Consulta { Id = id });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(Guid id)
        {
            return await Mediator.Send(new Eliminar.Ejecuta { CuentaId = id });
        }
    }
}
