using Microsoft.AspNetCore.Mvc;
using Models;
using Proyecto_Final.Servicio;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Proyecto_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class miController : ControllerBase
    {
        public PruebaServicio servicio { get; set; }
        public miController(PruebaServicio miServicio)
        {
            servicio = miServicio;
        }

        /******************************************************Clientes**/
        [HttpPost("NuevoCliente/{id}/{nombre}")]
        public ActionResult CrearCliente(string id, string nombre)
        {
            var nuevoCliente = new Clientes(id, nombre);
            servicio.AgregarClientes(nuevoCliente);
            return Ok(nuevoCliente);
        }   


        [HttpGet("VerClientes")]
        public ActionResult<IEnumerable<Clientes>> PruebaClientes()
        {
            return Ok(servicio.MostrarClientes());
        }
        [HttpGet("BuscarClientes/{id}")]//buscar clienes por id
        public ActionResult<Clientes> BuscarClienteXId(string id) {
            var servicioBuscar = servicio.BuscarCliente(id);
            return Ok(servicioBuscar);

        }


        /*******************************************************Tarjetas***/
        [HttpPost("NuevaTarjeta")]
        public ActionResult CrearTarjeta([FromBody] Tarjeta tarjeta)
        {
            servicio.AgregarTarjeta(tarjeta);
            return Ok(tarjeta);
        }   

        [HttpGet("VerTarjetas")]
        public ActionResult<IEnumerable<Tarjeta>> PruebaTarjetas()
        {
            return Ok(servicio.MostrarTarjetas());
        }
        [HttpGet("BuscarTarjetas/{idCliente}")]
        public ActionResult<Tarjeta> BuscarTarjetaXid(string idCliente) {
            return Ok(servicio.BuscarTarjeta(idCliente));
        }
        //*cambio de pin*/
        [HttpPut("CambioPin/{id}/{pin}")]
        public ActionResult<Tarjeta> CambiarPin(string id, int pin)
        {
            var servicioCambio = servicio.CambiarPin(id, pin);
            return Ok(servicioCambio);
        }

        /*ver el balance de la tarjeta*/
        [HttpGet("verBalance/{idCliente}")]
        public ActionResult<Tarjeta> VerBalance(string idCliente)
        {
            var servicioVer = servicio.verBalanceTarjeta(idCliente);
            return Ok(servicioVer);
        }

        //*como prueba -seria mejor ponerlo en el constructor del servicio*//
        /*
        [HttpGet("Actualizar_Balance")]
        public ActionResult<Tarjeta> ActualizarBalance()
        {
            var servicioActualizar = servicio.ActualizarBalane();
            return Ok(servicioActualizar);
        }*/

        /*****************************************Transacciones*/
        [HttpGet("verTransacciones")]
        public ActionResult<IEnumerable<Transaccion>> VerTransacciones()
        {
            var servicioTransacciones = servicio.MostrarTransacciones();
            return Ok(servicioTransacciones);
        }

        [HttpPost("nuevaTransaccion")]
        public IActionResult RealizarTransacion(Transaccion trx) {
            bool nuevaTRX = servicio.RealizarTransaccion(trx);
            if (!nuevaTRX) {
                return BadRequest("Error en la Transaccio");
            }
            return Ok("Transaccion Realizada");
        }
    }
}
