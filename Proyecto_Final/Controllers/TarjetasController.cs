using Microsoft.AspNetCore.Mvc;
using Models;
using Proyecto_Final.DTOs;
using Proyecto_Final.Models;
using Proyecto_Final.Servicio;

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

        [HttpGet("ObtnerTarjetaxNumero{numeroTarjeta}")]
        public ActionResult<Tarjeta> ObtenerTarjetaxNumero(string numeroTarjeta)
        {
            var c = tarjetaServicio.BuscarTarjetaxNumero(numeroTarjeta);
            if (c == null) { return NotFound(); }
            return Ok(c);
        }


        [HttpGet("verTarjetas")]
        public ActionResult<IEnumerable<Tarjeta>> OtenerTarjetas()
        {
            return  Ok(tarjetaServicio.ObtenerTarjetas());
        }

        [HttpGet("verBalance/{numeroTarjeta}")]
        public ActionResult<string> VerBalance(string numeroTarjeta)
        {
            var tarjeta = tarjetaServicio.verBalance(numeroTarjeta);
            return Ok(tarjeta);

        }

        [HttpPut("cambiarPin/{numeroTarjeta}")]
        public ActionResult CambiarPin(string numeroTarjeta, [FromBody] int nuevoPin)
        {

            var tarjeta = tarjetaServicio.CabioPin(numeroTarjeta, nuevoPin);   
            return Ok(tarjeta);
        }

        [HttpPut("BloquearTarjeta")]
        public ActionResult BloquearTarjeta(string numTarjeta) {
            var tarjeta = tarjetaServicio.BloquearTarjeta(numTarjeta);
            if (tarjeta == null)
            {
                return NotFound("No se encuentra Disponible esa tarjeta");
            }

            return Ok(tarjeta);
        }

        [HttpDelete("DesbloquearTarjeta")]
        public ActionResult DesbloquearTarjeta(string numeroTarjeta)
        {
            var tarjeta = tarjetaServicio.DesbloquearTarjeta(numeroTarjeta);
            if (tarjeta == null) { 
                return NotFound("No se encuentra Disponible esa tarjeta"); 
            }
            return Ok(tarjeta);
        }

        [HttpPut("aumentoLimite/{numeroTarjeta}")]
        public ActionResult AumentoLimite(string numeroTarjeta, [FromBody] decimal nuevoLimite)
        {
            var tarjeta = tarjetaServicio.AumentoLimite(numeroTarjeta, nuevoLimite);
            if (tarjeta == null)
            {
                return NotFound("No se encuentra Disponible esa tarjeta");
            }
            return Ok(tarjeta);
        }

        [HttpGet("HistorialLimites/{numeroTarjeta}")]
        public ActionResult<IEnumerable<CambioLimiteTarjeta>> HistorialLimites(string numeroTarjeta)
        {
           var Historial = tarjetaServicio.verCambios_LimiteCredito(numeroTarjeta);
            if (Historial == null)
            {
                return NotFound("No se encuentra Disponible esa tarjeta");
            }
            return Ok(Historial);
        }

    }
}
