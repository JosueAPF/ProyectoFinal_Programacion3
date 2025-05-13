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
        public string BuscarTarjetaxNumero(string nume)
        {
            var tarjetas = ContextoEstructuras.colaTarjetas.ObtenerTodo();
            foreach (var item in tarjetas)
            {
                if (item.Numero == nume)
                {
                    return item.ToString();
                }
            }
            return null;
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

    }
}
