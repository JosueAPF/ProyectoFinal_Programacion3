namespace Proyecto_Final.DTOs
{
    public class TarjetaDTO_sinTransacciones
    {
        public string Id { get; set; }
        public string Numero { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public int Cvv { get; set; }
        public int Pin { get; set; }
        public decimal Balance { get; set; }
        public decimal LimiteCredito { get; set; }
        public bool IsBlocked { get; set; }
        public string IdCliente { get; set; }
    }
}
