using Models;
using Proyecto_Final.ArmadoEstructuras;
using Proyecto_Final.DTOs;
using Proyecto_Final.Models;

namespace Proyecto_Final.Servicio
{
    public class ClientesServicio
    {

        public ContextDatos ContextoEstructura { get; set; }
   
        public TarjetaServicio tarServicio { get; set; }

        public ClientesServicio(ContextDatos contexto, TarjetaServicio servicioTar)
        {
            ContextoEstructura = contexto;
            tarServicio = servicioTar;
        }

        public void AgregarClientes(Clientes cliente)
        {
            ///ContextoEstructura.colaClientes.Encolar(cliente);
            ContextoEstructura.abblClientes.Insertar(cliente);
            ContextoEstructura.tablaClientes.Insertar(cliente);
        }
        public IEnumerable<Clientes> ObtenerClientes()
        {
            return ContextoEstructura.abblClientes.ObtenerTodo();

        }


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

            var nueva = new Clientes(id, cliNombre.Name, viejo.Dato.DPI);
            var Modificacion = ContextoEstructura.abblClientes.ModificarNodo(id, nueva);
            //a prueba
            var ModificarTabla = ContextoEstructura.tablaClientes.Modificar(id, nueva);
            return nueva;

        }
        public void agregarTarjeta(Tarjeta tarjeta)
        {
            tarServicio.AgregarTarjeta(tarjeta);
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

            tarServicio.EliminarTarjeta(BuscandoClinetElim.Dato.Id);

            Clientes cliente = BuscandoClinetElim.Dato;
            if (cliente == null) { 
                return false; 
            }

            // Eliminar tarjetas asociadas del cliente en colaTarjetas
            var tarjetas = ContextoEstructura.colaTarjetas.ObtenerTodo();
            List<string> tarjetasAEliminar = new List<string>();//cambiar por una estructura aceptada
            
            //recorremos tarjetasa asociadas a los clientes existentes
            foreach (var tarjeta in tarjetas)
            {
                if (tarjeta.IdCliente == cliente.Id)
                {
                    tarjetasAEliminar.Add(tarjeta.Id);
                    ContextoEstructura.AvlTarjetasElim.Insertar(tarjeta);
                }
            }

            foreach (var idTarjeta in tarjetasAEliminar)
            {
                ContextoEstructura.colaTarjetas.Desencolar_TipoLista(idTarjeta);
            }




            ContextoEstructura.abblClientes.Eliminar(BuscandoClinetElim.Dato);
            ContextoEstructura.tablaClientes.Eliminar(BuscandoClinetElim.Dato.Id);

            //eliminado 
            return true;
        }

        


    }
}
