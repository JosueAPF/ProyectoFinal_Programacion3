using Models;

namespace Proyecto_Final.Models
{
    public class CambioLimiteTarjeta:AccesoId
    {
        public string Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal LimiteActual{get;set;}
        public decimal LimiteAnterior { get; set; }

        public CambioLimiteTarjeta(string id,DateTime fecha, decimal limiteAct, decimal limiteAnt)
        {
            Id = id;
            Fecha = fecha;
            LimiteActual = limiteAct;
            LimiteAnterior = limiteAnt;
        }

        public override string ToString()
        {
            return $"\n\tFecha :{Fecha}" +
                    $"\n\tLimite Actual : {LimiteActual}" +
                    $"\n\tLimite Anterior :{LimiteAnterior}";
            ;
        }
    }
}
