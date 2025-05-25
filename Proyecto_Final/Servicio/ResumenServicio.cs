using System.Text;
using Proyecto_Final.ArmadoEstructuras;

namespace Proyecto_Final.Servicio
{
    public class ResumenServicio
    {

        public ContextDatos contexto { get; set; }

        public ResumenServicio(ContextDatos servicio)
        {
            contexto = servicio;
        }

        public string ResumenCliente(string idCliente)
        {
            var nodo = contexto.tablaClientes.BuscarTabla(idCliente);
            if (nodo == null) {
                return "Cliente no encontrado!";
            }
            var cliente = nodo.Dato;

            

            StringBuilder resumen = new StringBuilder();
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
                foreach (var pagos in contexto.ListaPagos.ObtenerTodo())
                {
                    if (pagos.Numero.Equals(tarjeta.Numero)) {
                        if (pagos == null) {
                            resumen.AppendLine("no hay pagos aun");
                        }
                        resumen.AppendLine("---Pagos---")
                            .AppendLine($"Entidad Bancaria :{pagos.Establecimiento}")
                            .AppendLine($"Pago Cantidad :{pagos.Monto}")
                            .AppendLine($"Fecha y Hora :{pagos.FechaTransaccion}");
                    }
                }

                foreach (var compras in contexto.ListaCompras.ObtenerTodo())
                {
                    if (compras.Numero.Equals(tarjeta.Numero))
                    {
                        if (compras == null)
                        {
                            resumen.AppendLine("no hay pagos aun");
                        }
                        resumen.AppendLine("---Pagos---")
                            .AppendLine($"Entidad Bancaria :{compras.Establecimiento}")
                            .AppendLine($"Pago Cantidad :{compras.Monto}")
                            .AppendLine($"Fecha y Hora :{compras.FechaTransaccion}");
                    }
                }
            }


            return resumen.ToString();
        }

    }


}
