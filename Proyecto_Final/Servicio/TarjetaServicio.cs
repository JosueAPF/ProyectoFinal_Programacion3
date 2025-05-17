using Models;
using Proyecto_Final.ArmadoEstructuras;

namespace Proyecto_Final.Servicio
{
    public class TarjetaServicio
    {
        public ContextDatos ContextoEstructuras { get; set; }
        public TarjetaServicio(ContextDatos contexto)
        {
            ContextoEstructuras = contexto;
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
        public string verBalance(string idTarjeta) {
            decimal miSaldo = 0m;    
            foreach (var item in ContextoEstructuras.colaTarjetas.ObtenerTodo())
            {
                if (item.Id.Equals(idTarjeta)) { 
                    miSaldo = item.Balance;
                    if (item.IsBlocked) {
                        return $"Tarjeta ID :{idTarjeta}, Esta Tarjeta Esta Bloqueda";
                    }
                } 
            }
            return $"Tarjeta ID :{idTarjeta}, Saldo :{miSaldo}"; 
        }

        /*Prueba #1 cambio de pin**/
        public string CabioPin(string idTarjeta, int nuevoPin)
        {
            int CambiosRealizados = 0;
            foreach (var item in ContextoEstructuras.colaTarjetas.ObtenerTodo())
            {
                if (item.Id.Equals(idTarjeta))
                {
                    item.Pin = nuevoPin;
                    CambiosRealizados++;

                    //cambios Permitidos
                    if (CambiosRealizados > 2)
                    {
                        return $"Tarjeta ID :{idTarjeta}, Limite de Cambios Vuelva a interntarlo";
                    }
                }
            }
            return $"Tarjeta Id :{idTarjeta}, Nuevo Pin {nuevoPin}";
        }
    }
}
