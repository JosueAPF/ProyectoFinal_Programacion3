using Models;

namespace Proyecto_Final.DTOs
{
    public class ClienteDTO_Tarjeta
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DPI { get; set; }
        public List<TarjetaDTO_sinTransacciones> Tarjetas { get; set; }= new List<TarjetaDTO_sinTransacciones>();
    }
}
