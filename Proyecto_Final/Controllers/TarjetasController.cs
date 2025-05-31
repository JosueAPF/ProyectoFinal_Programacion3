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

            var addTarjeta = tarjetaServicio.AgregarTarjeta(tarjetaNueva);
            
            if (!addTarjeta) {
                return BadRequest($"Error Cliente {tarjeta.IdCliente} no existe!");
            }

            return Created();

        }
        
        [HttpGet("ObtnerTarjetaxId/{idTarjeta}")]
        public  ActionResult<Tarjeta> ObtenerTarjetaxId(string idTarjeta)
        {
            var tar = tarjetaServicio.BuscarTarjetaxId(idTarjeta);

            if (tar == null) {
                    return NotFound($"Esa Tarjeta no Existe {idTarjeta}"); 
            }
           return Ok(tar);
        }

        [HttpGet("ObtnerTarjetaxNumero{numeroTarjeta}")]
        public ActionResult<Tarjeta> ObtenerTarjetaxNumero(string numeroTarjeta)
        {
            var tar = tarjetaServicio.BuscarTarjetaxNumero(numeroTarjeta);
            if (tar == null) { 
                return NotFound($"Esa Tarjeta no Existe {numeroTarjeta}"); 
            }
            return Ok(tar);
        }


        [HttpGet("verTarjetas")]
        public ActionResult<IEnumerable<Tarjeta>> OtenerTarjetas()
        {
            return  Ok(tarjetaServicio.ObtenerTarjetas());
        }

        [HttpDelete("EliminarTarjeta/{numeroTarjeta}")]
        public ActionResult<string> ElimnarTarjeta(string numeroTarjeta) {
            var existencia = tarjetaServicio.BuscarTarjetaxNumero(numeroTarjeta);
            
            if (existencia != null) { 
                return Ok(tarjetaServicio.EliminarTarjeta(numeroTarjeta));
            
            }
            return BadRequest($"el numero de Tarjeta {numeroTarjeta} es incorrecto!");
        }

        [HttpGet("TarjetasEliminadas")]
        public ActionResult<IEnumerable<Tarjeta>> TarjetasEliminadas()
        {
            return Ok(tarjetaServicio.verTarjetasEliminadas());
        }

        [HttpGet("verSaldo/{numeroTarjeta}")]
        public ActionResult<string> VerBalance(string numeroTarjeta)
        {
            var existencia = tarjetaServicio.BuscarTarjetaxNumero(numeroTarjeta);
            var tarjeta = tarjetaServicio.SaldoDisponible(numeroTarjeta);
            if (existencia != null) { 
                return Ok(tarjeta);
            
            }

            return BadRequest($"el numero de Tarjeta {numeroTarjeta} es incorrecto!");

        }

        [HttpPut("cambiarPin/{numeroTarjeta}")]
        public ActionResult CambiarPin(string numeroTarjeta, [FromBody] int nuevoPin)
        {
            var existencia = tarjetaServicio.BuscarTarjetaxNumero(numeroTarjeta);
            var tarjeta = tarjetaServicio.CabioPin(numeroTarjeta, nuevoPin);
            if (existencia != null)
            {
                return Ok(tarjeta);

            }
            return BadRequest($"el numero de Tarjeta {numeroTarjeta} es incorrecto!"); 
        }

        [HttpPut("BloquearTarjeta")]
        public ActionResult BloquearTarjeta(string numeroTarjeta) {
            var existencia = tarjetaServicio.BuscarTarjetaxNumero(numeroTarjeta);
            var bloqueando = tarjetaServicio.BloquearTarjeta(numeroTarjeta);
            if (existencia == null)
            {
                return BadRequest($"el numero de Tarjeta {numeroTarjeta} es incorrecto!");
            }

            return Ok(bloqueando);
        }

        [HttpDelete("DesbloquearTarjeta")]
        public ActionResult DesbloquearTarjeta(string numeroTarjeta)
        {
            var existencia = tarjetaServicio.BuscarTarjetaxNumero(numeroTarjeta);
            var desbloqueando = tarjetaServicio.DesbloquearTarjeta(numeroTarjeta);
            if (existencia == null) { 
                return BadRequest($"el numero de Tarjeta {numeroTarjeta} es incorrecto!");
            }
            return Ok(desbloqueando);
        }
        [HttpPut("Renovacion/{numeroTarjeta}")]
        public ActionResult RenovarTarjeta(string numeroTarjeta) {
            var existencia = tarjetaServicio.BuscarTarjetaxNumero(numeroTarjeta);
            var renovarT = tarjetaServicio.RenovarTarjeta(numeroTarjeta);
            if (existencia == null) {
                return BadRequest($"el numero de Tarjeta {numeroTarjeta} es incorrecto!");
            }
            return Ok(renovarT);
        }

        [HttpPut("aumentoLimite/{numeroTarjeta}")]
        public ActionResult AumentoLimite(string numeroTarjeta, [FromBody] decimal nuevoLimite)
        {
            var existencia = tarjetaServicio.BuscarTarjetaxNumero(numeroTarjeta);
            var aumento = tarjetaServicio.AumentoLimite(numeroTarjeta, nuevoLimite);
            if (existencia == null)
            {
                return BadRequest($"el numero de Tarjeta {numeroTarjeta} es incorrecto!");
            }
            return Ok(aumento);
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

        [HttpGet("verPagos/{numeroTarjeta}")]
        public ActionResult<IEnumerable<Transaccion>> verPagos(string numeroTarjeta) {
            var tarjeta = tarjetaServicio.BuscarTarjetaxNumero(numeroTarjeta);
            if (tarjeta == null) {
                return BadRequest($"{numeroTarjeta}, no se encuentra en el Sistema");
            }

            var MisPagos = tarjetaServicio.verPagos(numeroTarjeta);
            if (MisPagos == null) {
                return BadRequest("Datos no cargados o genere un Pago");
            }

            return Ok(MisPagos);
        
        }
        [HttpGet("verCompras/{numeroTarjeta}")]
        public ActionResult<IEnumerable<Transaccion>> verCompras(string numeroTarjeta)
        {
            var tarjeta = tarjetaServicio.BuscarTarjetaxNumero(numeroTarjeta);
            if (tarjeta == null)
            {
                return BadRequest($"{numeroTarjeta}, no se encuentra en el Sistema");
            }

            var MisCompras = tarjetaServicio.verCompras(numeroTarjeta);
            if (MisCompras == null)
            {
                return BadRequest("Datos no cargados o Genere una Compra");
            }

            return Ok(MisCompras);  


        }

    }
}
