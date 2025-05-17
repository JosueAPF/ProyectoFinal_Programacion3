using Microsoft.AspNetCore.Mvc;
using Models;
using Proyecto_Final.Servicio;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Proyecto_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransaccionController : ControllerBase
    {

        public TransaccionServicio TransServicio { get; set; }
        public TarjetaServicio TarjetaServicio { get; set; }
        public TransaccionController(TransaccionServicio servicio,TarjetaServicio tarjetaServicio)
        {
            TransServicio = servicio;
            TarjetaServicio = tarjetaServicio;  
        }

        [HttpPost("nuevaTransaccion")]
        public ActionResult<Transaccion> NuevaTransaccion([FromBody] Transaccion trx) {
            TransServicio.NuevaTransaccion(trx);
            var Tarjeta = TarjetaServicio.BuscarTarjetaxNumero(trx.Numero);
            if (Tarjeta != null) { 
            
                TarjetaServicio.agregarTransaccion(trx);    
            }
            return CreatedAtAction(nameof(ObtenerTransaccion), new { id = trx.Id }, trx);

        }

        [HttpGet("procesarPendiente")]
        public ActionResult ProcesarPendiente()
        {
            var procesado = TransServicio.ProcesarTransaccion();
            if (!procesado) { 
                return NotFound("No hay Pendientes por Procesar");
            }
            
            return Ok("se ha Procesado El Pendiente");
        }

        [HttpGet("procesarBatch")]
        public ActionResult ProcesarloTodo()
        {
            TransServicio.ProcesarTodo();
            return Ok("se ha Procesado Todo El Batch");
        }


        [HttpPost("obtenerPendientes")]
        public ActionResult<IEnumerable<Transaccion>> ObtenerPendientes()
        {
            return Ok(TransServicio.ObtenerPendientes_Cola());
        }

        [HttpPost("obtenerRecientes")]
        public ActionResult<IEnumerable<Transaccion>> ObtenerRecientes(int cantidadAPVer)
        {
            return Ok(TransServicio.ObtenerRecientes_Pila(cantidadAPVer));
        }



        /*Uso del Arbol Binario de Busqueda**/
        [HttpGet("verTransacciones")]
        public ActionResult<IEnumerable<Transaccion>> VerTodas()
        {
            return Ok(TransServicio.ObtenerTransaciones());
        }

        [HttpGet("buscarTransaccion/{id}")]
        public ActionResult<IEnumerable<Transaccion>> ObtenerTransaccion(string id)
        {
            var c = TransServicio.BuscarTransaccion(id);
            if (c == null) return NotFound();
            return Ok(c);
        }


    }
}
