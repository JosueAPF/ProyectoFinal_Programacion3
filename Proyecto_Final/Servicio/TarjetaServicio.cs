using Estructuras;
using Models;
using Proyecto_Final.ArmadoEstructuras;
using Proyecto_Final.Models;

namespace Proyecto_Final.Servicio
{
    //por arreglar :
            /*
             cuando se ingresan valores inexistentes en la mayoria de los endpoints 
             no se esta retornando ningun mensaje
             

             que mas pide el enunciado por agregar a tarjetas
             
             */

    public class TarjetaServicio
    {
        public ContextDatos ContextoEstructuras { get; set; }
        public Cola<CambioLimiteTarjeta> HistorialLimites { get; set; }
        public TarjetaServicio(ContextDatos contexto)
        {
            ContextoEstructuras = contexto;
            HistorialLimites = new Cola<CambioLimiteTarjeta>();
        }

        /*Metodos CRUD*/

        public IEnumerable<Tarjeta> ObtenerTarjetas()
        {
            return ContextoEstructuras.colaTarjetas.ObtenerTodo();
           
        }   

        public void AgregarTarjeta(Tarjeta tarjeta)
        {
            ContextoEstructuras.colaTarjetas.Encolar(tarjeta);
        }

       

        /*Busqueda Por Id*/
        public string BuscarTarjetaxId(string id)
        {
            return ContextoEstructuras.colaTarjetas.Buscar(id);
        }

        /*Busqueda por Numero de Tarjeta*/
        public Tarjeta BuscarTarjetaxNumero(string nume)
        {
            var tarjetas = ContextoEstructuras.colaTarjetas.ObtenerTodo();
            Tarjeta tarjeta = null;   
            foreach (var item in tarjetas)
            {
                if (item.Numero == nume)
                {
                    tarjeta =  item;
                }
            }
            return tarjeta;
        }

        public void agregarTransaccion(Transaccion trx)
        {
            foreach (var item in ContextoEstructuras.colaTarjetas.ObtenerTodo())
            {
                if (item.Numero == trx.Numero)
                {
                    item.AgregarTransaccion(trx);
                }
            }
        }
        /*Prueba #2 balance*/
        public string verBalance(string numTarjea) {
            var tarjeta = BuscarTarjetaxNumero(numTarjea);
            if (tarjeta == null) {
                return $"tarjeta {tarjeta} no Existe";
            }

            if (tarjeta.IsBlocked) {
                return $"tarjeta {tarjeta}, Esta Bloqueada";
            }
            
            return $"Id Tarjeta :{tarjeta.Id}, Saldo : Q{tarjeta.Balance}";

        }

        /*Prueba #1 cambio de pin**/
        public string CabioPin(string numtarjeta, int nuevoPin)
        {
            int CambiosRealizados = 0;
            var tarjeta = BuscarTarjetaxNumero(numtarjeta);

            if (tarjeta == null) {
                return $"tarjeta {numtarjeta} no Existe";
            }

            tarjeta.Pin = nuevoPin;
            CambiosRealizados++;

            
            return $"Tarjeta Id :{tarjeta.Id}, Numero Tarjeta: {tarjeta.Numero}, Nuevo Pin {nuevoPin}";
        }

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

        public bool AumentoLimite(string numeroTarjeta, decimal nuevoLimite) {
            var tarjeta = BuscarTarjetaxNumero(numeroTarjeta);

            if (tarjeta == null) {
                return false;
            }

            //creacion del registro Cola<>
            var cambio_agregarLimite = new CambioLimiteTarjeta(tarjeta.IdCliente,DateTime.UtcNow,nuevoLimite, tarjeta.LimiteCredito);


            //guardamos en el historial
            HistorialLimites.Encolar(cambio_agregarLimite);

            tarjeta.LimiteCredito = nuevoLimite;

            return true;
        }

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
    }
}
