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
                return NotFound();
            }
            return Ok(login);
        }
        [HttpGet("Clientes_Activos")]
        public ActionResult<IEnumerable<Clientes>> ClientesLoggeados() {

            return Ok(servicioUsuariosActivos.ObtenerUsuariosActivos());
        }

        [HttpGet("Logout/{idcliente}")]
        public ActionResult<bool> Logout(string idcliente)
        {
            var logout = servicioUsuariosActivos.deslogearUsuario(idcliente);
            if (logout == false)
            {
                return NotFound();
            }
            return Ok(logout);
        }   

        [HttpPost("nuevoClientes")]
        public void NuevoCliente([FromBody] ClienteDTO_Escritura cliDTO)
        {
            var cli = new Clientes(cliDTO.Id, cliDTO.Name, cliDTO.DPI);
            clientServicio.AgregarClientes(cli);
        }

        [HttpPost("nuevoClienteTarjeta")]
        public ActionResult NuevoClienteTarjeta([FromBody] ClienteDTO_Lectura cliDTO)
        {
            var cli = new Clientes(cliDTO.Id, cliDTO.Name, cliDTO.DPI);
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
            var cli = clientServicio.BuscarCliente(id);
            if (cli == null)
            {
                return NotFound("Ese Cliente No Existe !");
            }
            return Ok(cli);
        }
        

        [HttpGet("VerClientes")]
        public ActionResult<IEnumerable<Clientes>> ObtenerCliente()
        {
            var clientes = clientServicio.ObtenerClientes();
            return Ok(clientes);
        }

        /*
        [HttpGet("ObtenerTodo")]
        public ActionResult<IEnumerable<Clientes>>  ObtenerTodo() {
            var cliTar = clientServicio.ObtenerCliente_Tarjeta();
            return Ok(cliTar);
        
        }*/


        [HttpPut("ModificarNombre/{id}")]
        public ActionResult<Clientes> ModificarCliente(string id, [FromBody] ClienteDTO_ModificarNombre nuevoCliente)
        {
           var micliente = clientServicio.ModificarNombre(id, nuevoCliente);
            if (micliente == null) {

                return NotFound("Posiblemente El Cliente No Exista!");
            }
            return Ok(micliente);
        }

        [HttpDelete("EliminarCliente/{id}")]
        public ActionResult<string> EliminarCliente(string id)
        {
            var eliminar = clientServicio.ElimnarCliente(id);
            if (!eliminar) {
                return NotFound("Posiblemente El Cliente No Exista!");
            }
            return Ok(eliminar);
        }

        [HttpGet("Resumen/{id}")]
        public ActionResult verResumenCliente(string id) {
            var resumen = resServicio.ResumenCliente(id);    
            return Ok(resumen);
        
        }
    }
}
