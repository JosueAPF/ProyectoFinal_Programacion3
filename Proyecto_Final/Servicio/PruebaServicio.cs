using Proyecto_Final.Utils;
using Estructuras;
using Models;

namespace Proyecto_Final.Servicio
{

    /**********************Que hacer ********/

    /*Usar Data Context para para inyectar cada Dato Deserealizado con su respectiva Estructura*/
    /*en Program.Cs exponer el DataContext y los diferentes servicios*/
    /*Separar por servicios*/


    /****************************************/
    public class PruebaServicio
    {
        public Pila<Clientes> pilaClientes { get; set; } = new Pila<Clientes>();
        public Pila<Tarjeta> pilaTarjetas { get; set; } = new Pila<Tarjeta>();
        public Pila<Transaccion> pilaTransacciones { get; set; } = new Pila<Transaccion>();

        public PruebaServicio(DeserealizadorGenerico<Clientes> clientesDes,
            DeserealizadorGenerico<Tarjeta> tarjetasDes,
            DeserealizadorGenerico<Transaccion> transaccionesDes)
        {
            var clientesLectura = clientesDes.Lectura("Clientes.json");
            var TarjetasLectura = tarjetasDes.Lectura("Tarjetas.json");
            var TransaccionesLectura = transaccionesDes.Lectura("Transacciones.json");


            /***a sus respectivas estructuras de datos***/

            //llenado-Clientes
            foreach (Clientes itemCliente in clientesLectura)
            {
                pilaClientes.Push(itemCliente);
            }

            //llenado-Tarjetas
            foreach (Tarjeta itemTarjeta in TarjetasLectura)
            {
                pilaTarjetas.Push(itemTarjeta);
            }

            //llenado-Transacciones
            foreach (Transaccion itemTrans in TransaccionesLectura)
            {
                pilaTransacciones.Push(itemTrans);
            }

            
            RefrescarMisRelaciones();//actualiza balance, y relaciones(clientes a tarjetas, transacciones a tarjetas, etc)
        }

        /******************************************Clientes********************/
        public void AgregarClientes(Clientes cliente)
        {
            pilaClientes.Push(cliente);
        }

        public IEnumerable<Clientes> MostrarClientes()
        {

            return pilaClientes.ObtenerTodo();
        }
        public Clientes BuscarCliente(string id)
        {
            foreach (var busqueda_Clientes in pilaClientes.ObtenerTodo())
            {
                if (busqueda_Clientes.Id == id)
                {
                    return busqueda_Clientes;
                }
            }
            return null;
        }


        public void EliminarCliente(string id)
        {
            foreach (var busqueda_Clientes in pilaClientes.ObtenerTodo())
            {
                if (busqueda_Clientes.Id == id)
                {
                    pilaClientes.Pop();
                }
            }
        }


        /*************************************Tarjetas********************/
        public void AgregarTarjeta(Tarjeta tarjeta)
        {
            pilaTarjetas.Push(tarjeta);
            RefrescarMisRelaciones();

        }


        public IEnumerable<Tarjeta> MostrarTarjetas()
        {
            
            return pilaTarjetas.ObtenerTodo();
        }
        public string BuscarTarjeta(string id)
        {
            foreach (var busqueda_tarjetas in pilaTarjetas.ObtenerTodo())
            {
                if (busqueda_tarjetas.IdCliente == id)
                {
                    return busqueda_tarjetas.ToString();
                }
            }
            return "Cliente no encontrado";

        }
        /*Cambio de Pin*/
        public Tarjeta CambiarPin(string id, int nuevoPin)
        {
            foreach (var busqueda_tarjetas in pilaTarjetas.ObtenerTodo())
            {
                if (busqueda_tarjetas.Id == id)
                {
                    busqueda_tarjetas.Pin = nuevoPin;
                    return busqueda_tarjetas;
                }
            }
            return null;
        }

        /*ver Balance de Tarjetas*/
        public decimal verBalanceTarjeta(string idTarjeta)
        {
            RefrescarMisRelaciones();
            foreach (var itemTarjeta in pilaTarjetas.ObtenerTodo())
            {
                if (itemTarjeta.Id == idTarjeta)
                {
                    return itemTarjeta.Balance;
                }
            }
            return 0;
            

        }
        /*ver si esta bloqueado*/

        /*Actualizar blance con respecto a las trasnsacciones **/
        private void ActualizarBalane()
        {
            foreach (var itemTarjeta in pilaTarjetas.ObtenerTodo())
            {

                decimal miBalance = itemTarjeta.Balance;

                foreach (var itemTrans in pilaTransacciones.ObtenerTodo())
                {
                    if (itemTarjeta.Numero == itemTrans.Numero)
                    {
                        miBalance -= itemTrans.Monto;
                    }
                }

                if (miBalance <= 0m)
                {
                    itemTarjeta.Balance = 0m;
                    itemTarjeta.IsBlocked = true;
                }
                else
                {
                    itemTarjeta.Balance = miBalance;

                }
            }

        }



        /*************************************Transacciones********************/

        /*aun no funciona del todo hace mas pruebas aqui*/
        public bool RealizarTransaccion(Transaccion trx)
        {
            Tarjeta tarjeta = null;//para almacenar la iteracion de PilaTarjetas

            foreach (var itemTarjetas in pilaTarjetas.ObtenerTodo())
            {

                if (itemTarjetas.Numero == trx.Numero)
                {
                    tarjeta = itemTarjetas;
                    break;
                }
            }
            if (tarjeta == null)
            {
                throw new ArgumentException($"No existe tarjeta con número {trx.Numero}");
            }

            if (tarjeta.IsBlocked || tarjeta.Balance <= 0m)
            {
                return false;
            }

            /*aplicando la logica de Actualizar monto en nueva transaccion*/
            decimal nuevoSaldo = tarjeta.Balance - trx.Monto;
            if (nuevoSaldo <= 0m)
            {
                tarjeta.Balance = 0m;
                tarjeta.IsBlocked = true;
            }
            else { 
                tarjeta.Balance = nuevoSaldo;   

            }

            pilaTransacciones.Push(trx);
            tarjeta.Transacciones.Add(trx); 
            //RefrescarMisRelaciones();


            return true;
        }


        public IEnumerable<Transaccion> MostrarTransacciones()
        {
            RefrescarMisRelaciones();
            return pilaTransacciones.ObtenerTodo();
        }

/**********************************Utiles para insertar los valores a las listas correspondientes :*/
       private void AgregarTarjetasAClientes()
        {
            foreach (var itemCliente in pilaClientes.ObtenerTodo())
            {
                foreach (var itemTarjeta in pilaTarjetas.ObtenerTodo())
                {
                    if (itemCliente.Id == itemTarjeta.IdCliente)
                    {
                        itemCliente.tarjetas.Add(itemTarjeta);
                    }
                }
            }

        }

        private void AgregarTransaccionesATarjetas()
        {
            foreach (var itemTarjeta in pilaTarjetas.ObtenerTodo())
            {

                foreach (var itemTrans in pilaTransacciones.ObtenerTodo())
                {
                    if (itemTrans.Numero == itemTarjeta.Numero)
                    {
                        itemTarjeta.Transacciones.Add(itemTrans);
                    }
                }
            }

        }

        public void RefrescarMisRelaciones() {

            foreach (var itemCliente in pilaClientes.ObtenerTodo()) {
                itemCliente.tarjetas.Clear();
            }

            foreach (var itemTarjetas in pilaTarjetas.ObtenerTodo())
            {
                itemTarjetas.Transacciones.Clear();
            }

            /*agregando a sus respectivas listas*/
            AgregarTarjetasAClientes();
            AgregarTransaccionesATarjetas();
            ActualizarBalane();
        }
    }





}
