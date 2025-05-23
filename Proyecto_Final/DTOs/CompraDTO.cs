using Proyecto_Final.Models;

namespace Proyecto_Final.DTOs
{
    public class CompraDTO
    {
        public string Id { get; set; }
        public string Numero { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaTransaccion { get; set; }
        public Establecimiento Establecimiento { get; set; }
    }
}
