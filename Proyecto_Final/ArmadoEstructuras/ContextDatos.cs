using Proyecto_Final.Utils;
using Estructuras;
using Models;
using Proyecto_Final.Estructuras;


namespace Proyecto_Final.ArmadoEstructuras
{
    public class ContextDatos
    {
        public Cola<Clientes> colaClientes { get; set; }    
        public Cola<Tarjeta> colaTarjetas { get; set; }
        public Pila<Transaccion> pilaTransacciones { get; set; }

        /*arboles**/
        //clientes
        public ABB<Clientes> abblClientes { get; set; } = new ABB<Clientes>();
        //para Login y logout de usuarios
        public AVl<Clientes> avlClientesLogin { get; set; } = new AVl<Clientes>();

        //transacciones
        public ABB<Transaccion> abbTransacciones { get; set; } = new ABB<Transaccion>();

        public ContextDatos(DeserealizadorGenerico<Clientes> clientesDes,
            DeserealizadorGenerico<Tarjeta> tarjetasDes,
            DeserealizadorGenerico<Transaccion> transaccionesDes)
        {
         

            colaClientes = new Cola<Clientes>();
            colaTarjetas = new Cola<Tarjeta>();
            pilaTransacciones = new Pila<Transaccion>();

            /*arboles*/
            abblClientes = new ABB<Clientes>();
            avlClientesLogin = new AVl<Clientes>();
            abbTransacciones = new ABB<Transaccion>();  

            /***a sus respectivas estructuras de datos***/

            //llenado-Clientes
            foreach (var c in clientesDes.Lectura("Clientes.json"))
                colaClientes.Encolar(c);
            //llenado-Tajetas
            foreach (var t in tarjetasDes.Lectura("Tarjetas.json"))
                colaTarjetas.Encolar(t);
            //llenado-Transacciones
            foreach (var x in transaccionesDes.Lectura("Transacciones.json"))
                pilaTransacciones.Push(x);


            /*Arboles*/

            //ABB-clientes  
            foreach (var item in clientesDes.Lectura("Clientes.json"))
                abblClientes.Insertar(item);
           

            //ABB-Transacciones
            foreach (var item in transaccionesDes.Lectura("Transacciones.json"))
                abbTransacciones.Insertar(item);    

        }
    }
}
