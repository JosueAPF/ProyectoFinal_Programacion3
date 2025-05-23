/*
 
    Establecimiento es una lista para 
 
 */

namespace Proyecto_Final.Models
{
    public class Establecimiento
    {
        public string Id { get; set; }
        //public string IdTrx { get; set; }
        public string Nombre { get; set; }  
        public string Categoria { get; set; }

        public Establecimiento()
        {
            
        }

        public Establecimiento(string id,string nombre,string cat)
        {
            Id = id;
            //IdTrx = trx;    
            Nombre = nombre;
            Categoria = cat;    
        }

        public override string ToString()
        {
            return "" +
                   $"\n\tId :{Id}" +
                   $"\n\tNombre: {Nombre}," +
                   $"\n\tCategoria: {Categoria}";
        }
    }
}
