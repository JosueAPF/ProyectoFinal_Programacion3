using System.Text;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Proyecto_Final.ArmadoEstructuras;

namespace Proyecto_Final.Servicio
{
    public class ResumenServicio
    {

        public ContextDatos contexto { get; set; }
        public TarjetaServicio tarjetaservico { get; set; }

        public ResumenServicio(ContextDatos servicio, TarjetaServicio tarjetaservico)
        {
            contexto = servicio;
            this.tarjetaservico = tarjetaservico;
        }

        public string ResumenCliente(string idCliente)
        {
            var nodo = contexto.tablaClientes.BuscarTabla(idCliente);
            if (nodo == null)
            {
                return "Cliente no encontrado!";
            }
            var cliente = nodo.Dato;



            StringBuilder resumen = new StringBuilder();
            StringBuilder resumenP = new StringBuilder();
            StringBuilder resumenC = new StringBuilder();
            resumen.AppendLine($"Resumen del Cliente: {cliente.Name} (DPI: {cliente.DPI})\n");

            if (cliente.tarjetas.Count == 0)
            {
                return resumen.AppendLine("No tiene tarjetas asociadas.").ToString();
            }



            foreach (var tarjeta in contexto.colaTarjetas.ObtenerTodo())
            {

                if (tarjeta.IdCliente == cliente.Id)
                {
                    //almacenando la info para usarla despue
                    decimal deuda = tarjeta.deuda;
                    decimal limite = tarjeta.LimiteCredito;
                    decimal disponibl = limite - deuda;
                    bool bloqueada = tarjeta.IsBlocked;
                    var trx = tarjeta.Transacciones;



                    resumen.AppendLine($"-> Numero de Tarjeta :{tarjeta.Numero}").
                        AppendLine($"Deuda Actual :{deuda}")
                        .AppendLine($"Limite de Credito :{tarjeta.LimiteCredito}")
                        .AppendLine($"Saldo Actual (para gastar!) :{disponibl}")
                        .AppendLine($"Se encuentra Bloqueada :{bloqueada}")
                        .AppendLine("\n");

                    //pagos
                    foreach (var trans in trx)
                    {
                        if (trans == null)
                        {
                            resumen.AppendLine("No cuenta con pagos aun");
                        }
                        if (trans?.Tipo == TipoTransaccion.Pago)
                        {
                            resumenC.AppendLine("---Pago---")
                            .AppendLine($"Establecimiento  :{trans.Establecimiento.Nombre}")
                            .AppendLine($"Cantidad :{trans.Monto}")
                            .AppendLine($"Fecha y Hora :{trans.FechaTransaccion}")
                            .AppendLine($"Con Tarjeta :{trans.NueroTarjeta}")
                            .AppendLine("\n");
                        }
                    }

                    //compras
                    foreach (var trans in trx)
                    {
                        if (trans == null)
                        {
                            resumen.AppendLine("No se han Aprobado, o no existen registro aun");
                        }
                        if (trans?.Tipo == TipoTransaccion.Compra)
                        {
                            resumenC.AppendLine("---Compras---")
                            .AppendLine($"Establecimiento  :{trans.Establecimiento.Nombre}")
                            .AppendLine($"Cantidad :{trans.Monto}")
                            .AppendLine($"Fecha y Hora :{trans.FechaTransaccion}")
                            .AppendLine($"Con Tarjeta :{trans.NueroTarjeta}")
                            .AppendLine("\n");
                        }
                    }
                }

            }
            resumen.AppendLine("----------Transacciones-------------");


            return resumen.AppendLine(resumenP.ToString()).AppendLine(resumenC.ToString()).ToString();
        }

    }


}
