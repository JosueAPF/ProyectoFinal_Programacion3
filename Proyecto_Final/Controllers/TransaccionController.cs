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

        [HttpGet("verTransacciones")]
        public ActionResult<IEnumerable<Transaccion>> Get()
        {
            return Ok(TransServicio.ObtenerTransaciones());
        }

        [HttpGet("obtenerTransaccion/{id}")]
        public ActionResult<string> ObtenerTransaccion(string id)
        {
            var c = TransServicio.BuscarTransaccion(id);
            if (c == null) return NotFound();
            return Ok(c);
        }


    }
}
