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
                    decimal deuda = tarjeta.Balance;
                    decimal limite = tarjeta.LimiteCredito;
                    decimal disponibl = limite - deuda;
                    bool bloqueada = tarjeta.IsBlocked;



                    resumen.AppendLine($"-> Numero de Tarjeta :{tarjeta.Numero}").
                        AppendLine($"Deuda Actual :{deuda}")
                        .AppendLine($"Limite de Credito :{tarjeta.LimiteCredito}")
                        .AppendLine($"Saldo Actual (para gastar!) :{disponibl}")
                        .AppendLine($"Se encuentra Bloqueada :{bloqueada}")
                        .AppendLine("\n");

                }
                //transacciones no funciona
                var misPagos = tarjetaservico.verPagos(tarjeta.Numero);
                foreach (var pagos in misPagos)
                {
                    if (pagos == null)
                    {
                        resumenP.AppendLine("no hay pagos aun");
                    }
                    if (tarjeta.Numero.Equals(pagos.Numero))
                    {
                        resumenP.AppendLine("---Pagos---")
                               .AppendLine($"Entidad Bancaria :{pagos.Establecimiento}")
                               .AppendLine($"Pago Cantidad :{pagos.Monto}")
                               .AppendLine($"Fecha y Hora :{pagos.FechaTransaccion}")
                               .AppendLine($"Con Tarjeta :{pagos.Numero}");
                    }
                    else
                    {
                        resumenP.AppendLine("");
                    }

                }

                var misCompras = tarjetaservico.verCompras(tarjeta.Numero);
                foreach (var compras in misCompras)
                {
                    if (compras == null)
                    {
                        resumenC.AppendLine("no hay Copras aun");
                    }
                    if (tarjeta.Numero.Equals(compras.Numero))
                    {
                        resumenC.AppendLine("---Compras---")
                        .AppendLine($"Establecimiento  :{compras.Establecimiento}")
                        .AppendLine($"Cantidad :{compras.Monto}")
                        .AppendLine($"Fecha y Hora :{compras.FechaTransaccion}")
                        .AppendLine($"Con Tarjeta :{compras.Numero}");
                    }
                    else
                    {
                        resumenC.AppendLine("");
                    }
                }


            }
                resumen.AppendLine("-----------------------");


            return resumen.AppendLine(resumenP.ToString()).AppendLine(resumenC.ToString()).ToString();
        }

    }


}
