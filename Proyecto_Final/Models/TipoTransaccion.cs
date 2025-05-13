using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TipoTransaccion
    {
        Compra,
        Pago,
        Retiro,
        Transferencia,
        Intereses,
        Comision
    }
}
