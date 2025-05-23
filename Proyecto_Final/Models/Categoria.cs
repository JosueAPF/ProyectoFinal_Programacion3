using System.Text.Json.Serialization;

namespace Proyecto_Final.Models
{
    //[JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Categoria
    {
        Supermercado,
        Restaurantes,
        Gasolinera,
        Entretenimiento,
        Farmacia,
        Viajes,
        ComprasEnLinea,
        ServiciosPublicos,
        
    }
}
