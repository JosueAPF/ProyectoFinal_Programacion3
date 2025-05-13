/*
 
    Establecimiento es una lista para 
 
 */

namespace Proyecto_Final.Models
{
    public class Establecimiento
    {
        public string Id { get; set; }
        public string IdTrx { get; set; }
        public string Nombre { get; set; }  
        public Categoria Categoria { get; set; }

        public Establecimiento()
        {
            
        }

        public Establecimiento(string id,string trx,string nombre,Categoria cat)
        {
            Id = id;
            IdTrx = trx;    
            Nombre = nombre;
            Categoria = cat;    
        }

        public override string ToString()
        {
            return "" +
                   $"\n\tId :{Id}" +
                   $"\n\tId Transaccion :{IdTrx}"+
                   $"\n\tNombre: {Nombre}," +
                   $"\n\tCategoria: {Categoria}";
        }
    }
}
