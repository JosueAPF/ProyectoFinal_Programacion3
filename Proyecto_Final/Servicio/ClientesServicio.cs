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
            ///ContextoEstructura.colaClientes.Encolar(cliente);
            ContextoEstructura.abblClientes.Insertar(cliente);
        }
        public IEnumerable<Clientes> ObtenerClientes()
        {
            return ContextoEstructura.abblClientes.ObtenerTodo();

        }
        /*
        public IEnumerable<Clientes> ObtenerCliente_Tarjeta()
        {
            var clientes = ContextoEstructura.abblClientes.ObtenerTodo();
            var tar = ContextoEstructura.colaTarjetas.ObtenerTodo();

            foreach (var itemC in clientes)
            {
                itemC.EliminarTarjeta();
            }

            foreach (var itemT in tar)
            {
                foreach (var itemC in clientes)
                {

                    if (itemT.IdCliente == itemC.Id)
                    {
                        itemC.AgregarTarjeta(itemT);
                    }
                }
            }

            return clientes;
        }*/

        public string BuscarCliente(string id)
        {
            var buscarcliente = ContextoEstructura.abblClientes.Buscar(id);
            if (buscarcliente == null)
            {
                return null;
            }
            return buscarcliente.ToString();
        }
        /* Modifica : Version sin DTO*/
        public string ModificarCliente(string id, Clientes nuevoCliente)
        {
            var clienteModificado = ContextoEstructura.abblClientes.ModificarNodo(id, nuevoCliente);
            return clienteModificado.ToString();
        }


        public Clientes ModificarNombre(string id, ClienteDTO_ModificarNombre cliNombre)
        {
            var viejo = ContextoEstructura.abblClientes.Buscar(id);
            if (viejo == null)
            {
                return null;
            }

            var nueva = new Clientes(id, cliNombre.Name);
            var Modificacion = ContextoEstructura.abblClientes.ModificarNodo(id, nueva);
            return nueva;

        }
        public void agregarTarjeta(Tarjeta tarjeta)
        {
            foreach (var item in ContextoEstructura.abblClientes.ObtenerTodo())
            {
                if (item.Id == tarjeta.IdCliente)
                {
                    item.AgregarTarjeta(tarjeta);
                }
            }
        }
        public bool ElimnarCliente(string Id)
        {
            if (Id == null)
            {
                return false;
            }

            var BuscandoClinetElim = ContextoEstructura.abblClientes.Buscar(Id);

            if (BuscandoClinetElim == null)
            {
                return false;
            }

            ContextoEstructura.abblClientes.Eliminar(BuscandoClinetElim.Dato);

            //eliminado 
            return true;
        }


    }
}
