using Proyecto_Final.Utils;
using Estructuras;
using Models;
using Proyecto_Final.Estructuras;


namespace Proyecto_Final.ArmadoEstructuras
{
    public class ContextDatos
    {
        //pruebas
        public Cola<Clientes> colaClientes { get; set; }    
        public Cola<Tarjeta> colaTarjetas { get; set; }



        /*arboles**/
        //clientes
        public ABB<Clientes> abblClientes { get; set; } 
        //para Login y logout de usuarios
        public AVl<Clientes> avlClientesLogin { get; set; } 

        //transacciones-Prueba
        public ABB<Transaccion> abbTransacciones { get; set; } 
        public Cola<Transaccion> ColaTransacciones { get; set; }
        public Pila<Transaccion> pilaTransacciones { get; set; }

        public ContextDatos(DeserealizadorGenerico<Clientes> clientesDes,
            DeserealizadorGenerico<Tarjeta> tarjetasDes,
            DeserealizadorGenerico<Transaccion> transaccionesDes)
        {
         

            colaClientes = new Cola<Clientes>();
            colaTarjetas = new Cola<Tarjeta>();
            abbTransacciones = new ABB<Transaccion>();  
            pilaTransacciones = new Pila<Transaccion>();



            /*arboles*/
            abblClientes = new ABB<Clientes>();
            avlClientesLogin = new AVl<Clientes>();

            ColaTransacciones = new Cola<Transaccion>();    

            /***a sus respectivas estructuras de datos***/
            ////////////////////////esta parte es apra probar funcionalidades se debe eliminar
            //llenado-Clientes
            foreach (var c in clientesDes.Lectura("Clientes.json"))
                colaClientes.Encolar(c);
            //llenado-Tajetas
            foreach (var t in tarjetasDes.Lectura("Tarjetas.json"))
                colaTarjetas.Encolar(t);

            /******************************Datos iniciales y transacciones que si piede el enunciado*****************************/

            /*Arboles*/

            //ABB-clientes  
            foreach (var item in clientesDes.Lectura("Clientes.json")) { 
                abblClientes.Insertar(item);
            
            }

            /* ABB Y Cola *///incolucrado Tambien pila<transacciones> pero en el servicio
            //Transacciones:Datos iniciales
            foreach (var t in transaccionesDes.Lectura("Transacciones.json")) {
                abbTransacciones.Insertar(t);
                ColaTransacciones.Encolar(t);

            }




            /*Tarjetas y sus Transacciones*/
            /*
            foreach (var t in colaTarjetas.ObtenerTodo())
            {
                foreach (var trx in ColaTransacciones.ObtenerTodo())
                {
                    if (t.Numero.Equals(trx.Numero))
                    {
                        t.AgregarTransaccion(trx);
                    }

                }
            }*/


            
           

    
            


            //


          
           

            



        }
    }
}
