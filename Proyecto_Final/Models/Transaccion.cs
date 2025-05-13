using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Proyecto_Final.Models;

namespace Models
{
    public class Transaccion:AccesoId
    {

        public string Id { get; set; }
        
        public string Numero { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TipoTransaccion Tipo { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaTransaccion { get; set; }
        public Establecimiento Establecimiento { get; set; }

        public Transaccion()
        {
        }

        public Transaccion(string id, string number, TipoTransaccion tipo, decimal monto, DateTime fechaTransaccion, Establecimiento est )
        {
            Id = id;
            Numero = number;
            Tipo = tipo;
            Monto = monto;
            FechaTransaccion = fechaTransaccion;
            Establecimiento = est;  

        }

        public override string ToString()
        {
            return $"\n\tId: {Id}, " +
                $"\n\tNumber: {Numero}, " +
                $"\n\tTipo: {Tipo}, " +
                $"\n\tMonto: {Monto}, " +
                $"\n\tFecha: {FechaTransaccion}"+
                $"\n\tEstablecimiento: {Establecimiento}";
        }


      
    }
}
