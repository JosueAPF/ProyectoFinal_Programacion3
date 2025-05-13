using Models;
using Proyecto_Final.ArmadoEstructuras;
using Proyecto_Final.DTOs;
using Proyecto_Final.Models;

namespace Proyecto_Final.Servicio
{
    public class ClientesServicio
    {

        public ContextDatos ContextoEstructura { get; set; }
        public ClientesServicio(ContextDatos contexto)
        {
            ContextoEstructura = contexto;  
        }

        public void AgregarClientes(Clientes cliente)
        {
            ContextoEstructura.colaClientes.Encolar(cliente);
        }
        public IEnumerable<Clientes> ObtenerClientes() {
            return ContextoEstructura.colaClientes.ObtenerTodo();
        }

        public string BuscarCliente(string id)
        {
            return ContextoEstructura.colaClientes.Buscar(id);
        }
        /* Modifica : Version sin DTO*/
        public string ModificarCliente(string id, Clientes nuevoCliente) {
            var clienteModificado = ContextoEstructura.colaClientes.Modificar(id, nuevoCliente);
            return clienteModificado; 
        }


        public Clientes ModificarNombre(string id, ClienteDTO_ModificarNombre cliNombre) {
            var todosClientes = ContextoEstructura.colaClientes.ObtenerTodo();

            foreach (var item in todosClientes)
            {
                if (item.Id == id) { 
                    item.Name = cliNombre.Name;
                    return item;
                }
            }
            return null;

        }
        public void agregarTarjeta(Tarjeta tarjeta) {
           foreach(var item in ContextoEstructura.colaClientes.ObtenerTodo())
            {
                if (item.Id == tarjeta.IdCliente)
                {
                    item.AgregarTarjeta(tarjeta);
                }
            }   
        }

        
    }
}
