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

        public ResumenServicio resServicio { get; set; }


        public ServicioUsuariosActivos servicioUsuariosActivos { get; set; }
        public ClientesController(ClientesServicio servicio, TarjetaServicio tarjetaServicio, ServicioUsuariosActivos activos, ResumenServicio resServicio)
        {
            clientServicio = servicio;
            this.servicioUsuariosActivos = activos;
            this.tarjetaServicio = tarjetaServicio;
            this.resServicio = resServicio;
        }
        [HttpGet("login/{clienteId}")]
        public ActionResult<bool> Login(string clienteId)
        {
            var login = servicioUsuariosActivos.loginUsuario(clienteId);
            if (login == false)
            {
                return NotFound("Cliente Inexistente");
            }
            return Ok(login);
        }
        [HttpGet("Clientes_Activos")]
        public ActionResult<IEnumerable<Clientes>> ClientesLoggeados()
        {
            var listadoActivos = servicioUsuariosActivos.ObtenerUsuariosActivos();
            return Ok(listadoActivos);
        }

        [HttpDelete("Logout/{clienteId}")]
        public ActionResult<bool> Logout(string clienteId)
        {
            var logout = servicioUsuariosActivos.deslogearUsuario(clienteId);
            if (logout == false)
            {
                return NotFound("Cliente Inexistente");
            }
            return Ok(logout);
        }

        [HttpPost("nuevoClientes")]
        public ActionResult NuevoCliente([FromBody] ClienteDTO_Escritura cliDTO)
        {
            var cli = new Clientes(cliDTO.Id, cliDTO.Name, cliDTO.DPI);
            clientServicio.AgregarClientes(cli);
            return Created();//creado Exitosamente :)
        }

        [HttpPost("nuevoClienteTarjeta")]
        public ActionResult NuevoClienteTarjeta([FromBody] ClienteDTO_Lectura cliDTO)
        {

            // Validación: Asegurarse de que solo hay una tarjeta
            if (cliDTO.Tarjetas == null || cliDTO.Tarjetas.Count != 1)
            {
                return BadRequest("Debe proporcionar exactamente una tarjeta.");
            }

            var cli = new Clientes(cliDTO.Id, cliDTO.Name, cliDTO.DPI);
            clientServicio.AgregarClientes(cli);
           
            var tDto = cliDTO.Tarjetas.First();
            Tarjeta tarjeta = new Tarjeta(
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
            clientServicio.agregarTarjeta(tarjeta);

            return CreatedAtAction(nameof(ObtenerC),new { clienteId = cli.Id },cli);
        }

        [HttpGet("BuscarCliente/{clienteId}")]
        public ActionResult<Clientes> ObtenerC(string clienteId)
        {
            var cli = clientServicio.BuscarCliente(clienteId);
            if (cli == null)
            {
                return NotFound($"Posiblemente, {clienteId} El Cliente No Exista!");
            }
            return Ok(cli);
        }


        [HttpGet("VerClientes")]
        public ActionResult<IEnumerable<Clientes>> ObtenerCliente()
        {
            var clientes = clientServicio.ObtenerClientes();
            return Ok(clientes);
        }

        [HttpPut("ModificarNombre/{clienteId}")]
        public ActionResult<Clientes> ModificarCliente(string clienteId, [FromBody] ClienteDTO_ModificarNombre nuevoCliente)
        {
            var micliente = clientServicio.ModificarNombre(clienteId, nuevoCliente);
            if (micliente == null)
            {

                return NotFound($"Posiblemente, {clienteId} El Cliente No Exista!");
            }
            return Ok(micliente);
        }

        [HttpDelete("EliminarCliente/{clienteId}")]
        public ActionResult<string> EliminarCliente(string clienteId)
        {
            var eliminar = clientServicio.ElimnarCliente(clienteId);
            if (!eliminar)
            {
                return NotFound($"Posiblemente, {clienteId} El Cliente No Exista!");
            }
            return Ok(eliminar);
        }

        [HttpGet("Resumen/{clienteId}")]
        public ActionResult verResumenCliente(string clienteId)
        {
            var resumen = resServicio.ResumenCliente(clienteId);
            var Existencia = clientServicio.BuscarCliente(clienteId);
            if (Existencia != null)
            {
                return Ok(resumen);
            }
            return NotFound($"{clienteId} Inexistente!, el Clienet no Existe");

        }
    }
}
