using Models;
using Proyecto_Final.ArmadoEstructuras;

namespace Proyecto_Final.Servicio
{
    public class ServicioUsuariosActivos
    {
        public ContextDatos ContextoEstructuras { get; set; }

        public ServicioUsuariosActivos(ContextDatos contexto)
        {
            ContextoEstructuras = contexto;
        }

        public bool loginUsuario(string clienteId)
        {
            // Verifica la existencia del cliente en el árbol ABB
            var verificarCliente = ContextoEstructuras.abblClientes.Buscar(clienteId);

            // El cliente no existe
            if (verificarCliente == null)
            {
                return false;
            }

            // existe!!!!
            ContextoEstructuras.avlClientesLogin.Insertar(verificarCliente.Dato);
            return true;
        }

        // Los clientes activos
        public IEnumerable<Clientes> ObtenerUsuariosActivos()
        {
            return ContextoEstructuras.avlClientesLogin.ObtenerTodo();
        }
         
        public bool deslogearUsuario(string cliId)
        {
            
           var ExistenciaCliente = ContextoEstructuras.avlClientesLogin.BuscarRec(ContextoEstructuras.avlClientesLogin.Raiz,cliId);
            if (ExistenciaCliente == null) {
                return false;
            }

            ContextoEstructuras.avlClientesLogin.Eliminar(ExistenciaCliente.Dato);
            return true;
        }
    }
}
