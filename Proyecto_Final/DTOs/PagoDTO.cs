using Models;
using Proyecto_Final.Models;
using System.Text.Json.Serialization;

namespace Proyecto_Final.DTOs
{
    public class PagoDTO
    {
        public string Id { get; set; }
        public string Numero { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaTransaccion { get; set; }
        public Establecimiento Establecimiento { get; set; }
    }
}
