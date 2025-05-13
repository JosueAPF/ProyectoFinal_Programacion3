using Proyecto_Final.Utils;
using Estructuras;
using Models;


namespace Proyecto_Final.ArmadoEstructuras
{
    public class ContextDatos
    {
        public Cola<Clientes> colaClientes { get; set; }    
        public Cola<Tarjeta> colaTarjetas { get; set; }
        public Pila<Transaccion> pilaTransacciones { get; set; }

        public ContextDatos(DeserealizadorGenerico<Clientes> clientesDes,
            DeserealizadorGenerico<Tarjeta> tarjetasDes,
            DeserealizadorGenerico<Transaccion> transaccionesDes)
        {
            /*
            var clientesLectura = clientesDes.Lectura("Clientes.json");
            var TarjetasLectura = tarjetasDes.Lectura("Tarjetas.json");
            var TransaccionesLectura = transaccionesDes.Lectura("Transacciones.json");
            */

            colaClientes = new Cola<Clientes>();
            colaTarjetas = new Cola<Tarjeta>();
            pilaTransacciones = new Pila<Transaccion>();

            /***a sus respectivas estructuras de datos***/

            //llenado-Clientes
            foreach (var c in clientesDes.Lectura("Clientes.json"))
                colaClientes.Encolar(c);
            foreach (var t in tarjetasDes.Lectura("Tarjetas.json"))
                colaTarjetas.Encolar(t);
            foreach (var x in transaccionesDes.Lectura("Transacciones.json"))
                pilaTransacciones.Push(x);
        }
    }
}
