using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Models;

namespace Proyecto_Final.Utils
{

    public class DeserealizadorGenerico<T>
    {
        public Rutas misRutas { get; set; } //almacena la ruta absoluta
        public DeserealizadorGenerico(Rutas ruta)
        {
            misRutas = ruta;
        }
        public IEnumerable<T> Lectura(string nombreArchivo)
        {

            if (string.IsNullOrEmpty(nombreArchivo))
            {
                throw new ArgumentException("Esta mal el nombre!!!!");
            }

            var Archivo = misRutas.ObtenerArchivo(nombreArchivo);

            if (!File.Exists(Archivo))
            {
                throw new FileNotFoundException("El archivo no existe!!!!!!!!");
            }


            var LecturaArchivo = File.ReadAllText(Archivo);



            //Solucion al error de uso de enums para a clase Transacciones ya que usa enums
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,        
            };
            options.Converters.Add(new JsonStringEnumConverter());

            var ListaDeserealizadaXD = JsonSerializer.Deserialize<List<T>>(LecturaArchivo, options);
            if (ListaDeserealizadaXD == null) { 
                return new List<T>();   
            }

            return ListaDeserealizadaXD;//ojala que funcione

        }

    }

    //ya funciona :)
}
