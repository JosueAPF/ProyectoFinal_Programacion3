using Estructuras;
using Models;
using Proyecto_Final.ArmadoEstructuras;
using Proyecto_Final.Models;

namespace Proyecto_Final.Servicio
{

    public class TarjetaServicio
    {
        public ContextDatos ContextoEstructuras { get; set; }
        public Cola<CambioLimiteTarjeta> HistorialLimites { get; set; }
        public TarjetaServicio(ContextDatos contexto)
        {
            ContextoEstructuras = contexto;
            HistorialLimites = new Cola<CambioLimiteTarjeta>();
        }


        public IEnumerable<Tarjeta> ObtenerTarjetas()
        {
            return ContextoEstructuras.colaTarjetas.ObtenerTodo();

        }

        public bool AgregarTarjeta(Tarjeta tarjeta)
        {
            //agregando a las lista de el Modelo Clinetes
            
            var clientesaabb = ContextoEstructuras.abblClientes.Buscar(tarjeta.IdCliente);
            var clienteHash = ContextoEstructuras.tablaClientes.BuscarTabla(tarjeta.IdCliente);

            if (clienteHash == null && clientesaabb == null) {
                return false;
            }

            foreach (var item in ContextoEstructuras.abblClientes.ObtenerTodo())
            {
                //verificar que se cumpla El Cliente.id cliente de ser igual a tarjeta.IdCliente
                if (item.Id == tarjeta.IdCliente)
                {   
                    item.AgregarTarjeta(tarjeta);
                }
                
            }
            ContextoEstructuras.colaTarjetas.Encolar(tarjeta);
            return true;
        }

        /*Eliminar x Numero de tarjeta*/
        public string EliminarTarjeta(string numeroTarjeata) {
            var buscarTxID = BuscarTarjetaxNumero(numeroTarjeata);
            if (buscarTxID == null) {
                return $"la Tarjeta {buscarTxID}, no Existe!";
            }

            var NumeroTarjeta = buscarTxID.Numero;

            if (NumeroTarjeta == null) {
                return $"el Numero de  Tarjeta {buscarTxID.Numero}, no Existe!";    
            }
            var tarjeta = ContextoEstructuras.colaTarjetas.Desencolar_TipoLista(buscarTxID.Id);
            ContextoEstructuras.AvlTarjetasElim.Insertar(buscarTxID);
            return $"Eliminado con Exito, {tarjeta.Numero}";
        }
        //tarjetas eliminadas   
        public IEnumerable<Tarjeta> verTarjetasEliminadas()
        {
            return ContextoEstructuras.AvlTarjetasElim.ObtenerTodo();


        }

        /*Busqueda Por Id*/
        public Tarjeta BuscarTarjetaxId(string id)
        {
            var tarjetas = ContextoEstructuras.colaTarjetas.ObtenerTodo();
            Tarjeta tarjeta = null;
            foreach (var item in tarjetas)
            {
                if (item.Id == id)
                {
                    tarjeta = item;
                }
            }
           

            return tarjeta;
        }

        /*Busqueda por Numero de Tarjeta*/
        public Tarjeta BuscarTarjetaxNumero(string nume)
        {
            //iterando la cola con forEach gracias al IEnumerable<T>
            var tarjetas = ContextoEstructuras.colaTarjetas.ObtenerTodo();
            Tarjeta tarjeta = null;
            foreach (var item in tarjetas)
            {
                if (item.Numero == nume)
                {
                    tarjeta = item;
                }
            }
            return tarjeta;
        }

        //corregido ver Saldo Disponible
        //limite de credito - balance = saldo de Tarjeta
        public string SaldoDisponible(string numTarjeta)
        {
            var tarjeta = BuscarTarjetaxNumero(numTarjeta);
            if (tarjeta == null)
            {
                return "Tarjeta no encontrada";
            }
            if (tarjeta.IsBlocked)
            {
                return "Esta Tarjeta esta Bloqueada";
            }
            return $"Limite de Credito :Q{tarjeta.LimiteCredito} - Deuda : Q{tarjeta.Balance}  Saldo Disponible = Q{tarjeta.SaldoDisponible()}";
        }


        public string AumentoLimite(string numeroTarjeta, decimal nuevoLimite)
        {
            var tarjeta = BuscarTarjetaxNumero(numeroTarjeta);
            if (tarjeta == null)
            {
                return "Tarjeta No Encontrada!";
            }

            decimal LimitesPermitidos = tarjeta.LimiteCredito * 2m; //no mayores a 2 limites de credito
            if (LimitesPermitidos == null) {
                return "Error El Id no existe o dejo en Cero la casilla";
            }



            if (nuevoLimite < tarjeta.LimiteCredito)
            {
                return $"Este Limite {nuevoLimite}, no puede ser menor al limite anteriro {tarjeta.LimiteCredito}";
            }

            if (tarjeta.IsBlocked == true && tarjeta.Balance == 0m)
            {
                return $"La tarjeta se encuentra Bloqueada";
            }

            if (nuevoLimite > LimitesPermitidos)
            {
                return $"El nuevo limite no pude exceder los Limites Permitidos";
            }

            if (nuevoLimite < tarjeta.Balance)
            {
                return $"El nuevo Limite no puede ser menor a la deuda actual {tarjeta.Balance}";
            }

            //creacion del registro Cola<>
            var cambio_agregarLimite = new CambioLimiteTarjeta(tarjeta.IdCliente, DateTime.UtcNow, nuevoLimite, tarjeta.LimiteCredito);


            //guardamos en el historial
            HistorialLimites.Encolar(cambio_agregarLimite);

            tarjeta.LimiteCredito = nuevoLimite;

            return "Limite de Credito Actualizado!";
        }




        /*Prueba #1 cambio de pin**/
        public string CabioPin(string numtarjeta, int nuevoPin)
        {
            int CambiosRealizados = 0;
            var tarjeta = BuscarTarjetaxNumero(numtarjeta);

            if (tarjeta == null)
            {
                return $"tarjeta {numtarjeta} no Existe";
            }

            tarjeta.Pin = nuevoPin;
            CambiosRealizados++;


            return $"Tarjeta Id :{tarjeta.Id}, Numero Tarjeta: {tarjeta.Numero}, Nuevo Pin {nuevoPin}";
        }

        /**/
        public string BloquearTarjeta(string numtarjeta)
        {
            var tarjeta = BuscarTarjetaxNumero(numtarjeta);
            if (tarjeta == null)
            {
                return $"tarjeta {numtarjeta} no Existe";
            }
            tarjeta.IsBlocked = true;
            return $"Tarjeta Id :{tarjeta.Id}, Numero Tarjeta: {tarjeta.Numero}, Esta Bloqueada";
        }

        public string DesbloquearTarjeta(string numtarjeta)
        {
            var tarjeta = BuscarTarjetaxNumero(numtarjeta);
            if (tarjeta == null)
            {
                return $"tarjeta {numtarjeta} no Existe";
            }
            tarjeta.IsBlocked = false;
            return $"Tarjeta Id :{tarjeta.Id}, Numero Tarjeta: {tarjeta.Numero}, Esta Desbloqueada";
        }
        /*deuda*/
        public decimal verDeuda(string numtarjeta)
        {
            var tarjeta = BuscarTarjetaxNumero(numtarjeta);
            if (tarjeta == null)
            {
                return 0m;
            }
            return tarjeta.Balance;
        }


        /*Historial de Cambio de aumento de limite de una tarjeta*/
        public IEnumerable<CambioLimiteTarjeta>? verCambios_LimiteCredito(string idTarjeta)
        {
            if (HistorialLimites.estaVacio())
            {
                return Enumerable.Empty<CambioLimiteTarjeta>();
            }
            Tarjeta tarjeta = BuscarTarjetaxNumero(idTarjeta);
            if (tarjeta == null)
            {
                return Enumerable.Empty<CambioLimiteTarjeta>();
            }

            return HistorialLimites.ObtenerTodo();
        }

        //historial de pagos
        public void HistorialesTransacciones(string numTarjeta)
        {
            var tarjeta = BuscarTarjetaxNumero(numTarjeta);
            if (tarjeta == null)
            {
                return;
            }

            //esta inicial de las listas E
            ContextoEstructuras.ListaPagos.EliminarTodo();
            ContextoEstructuras.ListaCompras.EliminarTodo();
            var enumerador = tarjeta.Transacciones.GetEnumerator();


            foreach (var item in tarjeta.Transacciones)
            {
                if (item.Tipo == TipoTransaccion.Pago)
                {
                    if (tarjeta.Numero.Equals(item.Numero))
                    {
                        ContextoEstructuras.ListaPagos.Enlistar(item);
                    }
                    else {
                        return;
                    }

                }

                if (item.Tipo == TipoTransaccion.Compra)
                {
                    if (tarjeta.Numero.Equals(item.Numero))
                    {
                        ContextoEstructuras.ListaCompras.Enlistar(item);
                    }
                    else
                    {
                        return;
                    }
                }
            }


        }

        public IEnumerable<Transaccion> verPagos(string numtarjeta)
        {
            HistorialesTransacciones(numtarjeta);
            return ContextoEstructuras.ListaPagos.ObtenerTodo();
        }

        //historial de pagos
        public IEnumerable<Transaccion> verCompras(string numtarjeta)
        {
            HistorialesTransacciones(numtarjeta);
            return ContextoEstructuras.ListaCompras.ObtenerTodo();
        }

        //desencolado customizado para la cola (desencolado tipo ListaE)
        public string DesencolarTipoLista(string numeroTarjeta) {
            var buscarT = BuscarTarjetaxNumero(numeroTarjeta);
            ContextoEstructuras.colaTarjetas.Desencolar_TipoLista(buscarT.Id);

            return $"se elimino :{buscarT}";
        }
    }
}
