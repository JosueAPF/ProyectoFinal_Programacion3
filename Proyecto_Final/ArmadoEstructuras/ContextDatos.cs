using Proyecto_Final.Utils;
using Estructuras;
using Models;
using Proyecto_Final.Estructuras;
using TablaHash_E;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Proyecto_Final.ArmadoEstructuras
{
    public class ContextDatos
    {
        //cola de Tarjetas
        public Cola<Tarjeta> colaTarjetas { get; set; }

        //AVl de Tarjetas Eliminadas
        public AVl<Tarjeta> AvlTarjetasElim { get; set; }


        /// La tabla Hash
        public TablaHash<Clientes> tablaClientes { get; set; }


        /*arboles**/

        //clientes
        public ABB<Clientes> abblClientes { get; set; }
        //para Login y logout de usuarios
        public AVl<Clientes> avlClientesLogin { get; set; }

        //transacciones-Prueba
        public ABB<Transaccion> abbTransacciones { get; set; }
        public Cola<Transaccion> ColaTransacciones { get; set; }
        public Pila<Transaccion> pilaTransacciones { get; set; }

        /*para el historial de pagos y compras*/
        public ListaE_Simple<Transaccion> ListaPagos { get; set; }
        public ListaE_Simple<Transaccion> ListaCompras { get; set; }



        public ContextDatos(DeserealizadorGenerico<Clientes> clientesDes,
            DeserealizadorGenerico<Tarjeta> tarjetasDes,
            DeserealizadorGenerico<Transaccion> transaccionesDes)
        {


            //colaClientes = new Cola<Clientes>();
            //tarjetas CRUD
            colaTarjetas = new Cola<Tarjeta>();

            //clientes CRUD
            abbTransacciones = new ABB<Transaccion>();
            pilaTransacciones = new Pila<Transaccion>();

            /*historial de transacciones Lista Enlazada*/
            ListaPagos = new ListaE_Simple<Transaccion>();
            ListaCompras = new ListaE_Simple<Transaccion>();

            /*tarjetas Eliminadas*/
            AvlTarjetasElim = new AVl<Tarjeta>(); 


            /*Tabla hassh + Lista Enlazada simple*/
            tablaClientes = new TablaHash<Clientes>();

            /*arboles*/
            abblClientes = new ABB<Clientes>();
            avlClientesLogin = new AVl<Clientes>();
            ColaTransacciones = new Cola<Transaccion>();

           
            //llenado-Tajetas
            foreach (var t in tarjetasDes.Lectura("Tarjetas.json"))
                colaTarjetas.Encolar(t);

   

            /*Arboles*/

            //ABB-clientes  
            foreach (var item in clientesDes.Lectura("Clientes.json"))
            {
                abblClientes.Insertar(item);
                tablaClientes.Insertar(item);

            }
            
            //Transacciones:Datos iniciales
            foreach (var t in transaccionesDes.Lectura("Transacciones.json"))
            {
                abbTransacciones.Insertar(t);
                ColaTransacciones.Encolar(t);

            }


            /*Tabla Hash-Resumen para Clinetes tarjetas transacciones*/

           
            //complemento llenado de clinet y tarjeta para Clientes
            foreach (var t in tarjetasDes.Lectura("Tarjetas.json")) {
                var buscandoCli = tablaClientes.BuscarTabla(t.IdCliente);
                if (buscandoCli != null) {
                    buscandoCli.Dato.tarjetas.Add(t);   
                }
            }



        }
    }
}
