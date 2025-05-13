using Microsoft.AspNetCore.Mvc;
using Models;
using Proyecto_Final.DTOs;
using Proyecto_Final.Servicio;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Proyecto_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarjetasController : ControllerBase
    {
        public TarjetaServicio tarjetaServicio { get; set; }    
        public TarjetasController(TarjetaServicio servicio)
        {
            tarjetaServicio = servicio;
        }


        [HttpPost("nuevaTarjeta")]
        public ActionResult<Tarjeta> NewTarjeta([FromBody] TarjetaDTO_sinTransacciones tarjeta) { 
            var tarjetaNueva = new Tarjeta(
                id: tarjeta.Id,
                numero: tarjeta.Numero,
                fechaExpiracion: tarjeta.FechaExpiracion,
                cvv: tarjeta.Cvv,
                pin: tarjeta.Pin,
                balance: tarjeta.Balance,
                limiteCredito: tarjeta.LimiteCredito,
                estaBloqueada: tarjeta.IsBlocked,
                idCliente: tarjeta.IdCliente
            );

            tarjetaServicio.AgregarTarjeta(tarjetaNueva);
            return CreatedAtAction(nameof(ObtenerTarjetaxId), new { id = tarjeta.Id }, tarjetaNueva);

        }
        [HttpGet("ObtnerTarjetaxId{id}")]
        public  ActionResult<Tarjeta> ObtenerTarjetaxId(string id)
        {
            var c = tarjetaServicio.BuscarTarjetaxId(id);
            if (c == null) { return NotFound(); }
            return Ok(c);
        }

        [HttpGet("ObtnerTarjetaxNumero{Numero}")]
        public ActionResult<Tarjeta> ObtenerTarjetaxNumero(string Numero)
        {
            var c = tarjetaServicio.BuscarTarjetaxNumero(Numero);
            if (c == null) { return NotFound(); }
            return Ok(c);
        }


        [HttpGet("verTarjetas")]
        public ActionResult<IEnumerable<Tarjeta>> OtenerTarjetas()
        {
            return  Ok(tarjetaServicio.ObtenerTarjetas());
        }

       
    }
}
