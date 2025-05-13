using Models;
using Proyecto_Final.ArmadoEstructuras;

namespace Proyecto_Final.Servicio
{
    public class TransaccionServicio
    {
        public ContextDatos ContextoEstructuras { get; set; }
        public TransaccionServicio(ContextDatos datos)
        {
            ContextoEstructuras = datos;
        }

        public void NuevaTransaccion(Transaccion trx) {
            ContextoEstructuras.pilaTransacciones.Push(trx);
        }

        public IEnumerable<Transaccion> ObtenerTransaciones()
        {
            return ContextoEstructuras.pilaTransacciones.ObtenerTodo();
        }

        public string BuscarTransaccion(string id) { 
        
            return ContextoEstructuras.pilaTransacciones.Buscar(id);
        }

    }
}
