using Microsoft.AspNetCore.Mvc;
using Models;
using Proyecto_Final.DTOs;
using Proyecto_Final.Servicio;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Proyecto_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        public ClientesServicio clientServicio { get; set; }
        public TarjetaServicio tarjetaServicio { get; set; }
        public ClientesController(ClientesServicio servicio, TarjetaServicio tarjetaServicio)
        {
            clientServicio = servicio;
            this.tarjetaServicio = tarjetaServicio;
        }

        [HttpPost("nuevoClientes")]
        public void NuevoCliente([FromBody] ClienteDTO_Escritura cliDTO)
        {
            var cli = new Clientes(cliDTO.Id, cliDTO.Name);
            clientServicio.AgregarClientes(cli);
        }

        [HttpPost("nuevoClienteTarjeta")]
        public ActionResult NuevoClienteTarjeta([FromBody] ClienteDTO_Lectura cliDTO)
        {
            var cli = new Clientes(cliDTO.Id, cliDTO.Name);
            clientServicio.AgregarClientes(cli);

            foreach (var tDto in cliDTO.Tarjetas)
            {
                var tarjeta = new Tarjeta(
                id: tDto.Id,
                numero: tDto.Numero,
                fechaExpiracion: tDto.FechaExpiracion,
                cvv: tDto.Cvv,
                pin: tDto.Pin,
                balance: tDto.Balance,
                limiteCredito: tDto.LimiteCredito,
                estaBloqueada: false,
                idCliente: cli.Id
            );

                tarjetaServicio.AgregarTarjeta(tarjeta);
                clientServicio.agregarTarjeta(tarjeta);
            }
            return CreatedAtAction(nameof(ObtenerC),new { id = cli.Id },cli);
        }
        [HttpGet("BuscarCliente/{id}")]
        public ActionResult<Clientes> ObtenerC(string id)
        {
            var c = clientServicio.BuscarCliente(id);
            if (c == null) return NotFound();
            return Ok(c);
        }

        [HttpGet("VerClientes")]
        public ActionResult<IEnumerable<Clientes>> ObtenerCliente()
        {
            var clientes = clientServicio.ObtenerClientes();
            return Ok(clientes);
        }


        [HttpPut("ModificarNombre/{id}")]
        public ActionResult<Clientes> ModificarCliente(string id, [FromBody] ClienteDTO_ModificarNombre nuevoCliente)
        {
           var micliente = clientServicio.ModificarNombre(id, nuevoCliente);
            if (micliente == null) return NotFound();
            return Ok(micliente);
        }
    }
}
