using Aplicacion.Logica.Clientes;
using Aplicacion.Logica.Personas;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    // https://server:<PORT>/api/cliente
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : MiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await Mediator.Send(data);
        }

        [HttpGet]
        public async Task<ActionResult<List<PersonaDto>>> Obtener()
        {
            return await Mediator.Send(new Consulta.ListaClientes());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonaDto>> ObtenerPorId(Guid id)

        {
            return await Mediator.Send(new ConsultaPorId.Consulta { Id = id });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Editar(Guid id, Editar.Ejecuta data)
        {
            data.ClienteId = id;
            return await Mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(Guid id)
        {
            return await Mediator.Send(new Eliminar.Ejecuta { ClienteId = id });
        }
    }
}
