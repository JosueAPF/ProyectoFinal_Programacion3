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

        public bool loginUsuario(string clienteId) {
            //verifica la existencia de el cliente en el arbol ABB
            var verificarCliente = ContextoEstructuras.abblClientes.Buscar(clienteId);

            //el cliente no existe
            if (verificarCliente == null) {
                return false;
            }

            //como si existe el cliente, lo agrego al AVL 
            ContextoEstructuras.avlClientesLogin.Insertar(verificarCliente.Dato);    
            return true;    
            
        }
         // los clientes activos
        public IEnumerable<Clientes> ObtenerUsuariosActivos()
        {
            return ContextoEstructuras.avlClientesLogin.ObtenerTodo();
        }   
    }
}
