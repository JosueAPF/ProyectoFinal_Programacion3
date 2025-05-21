using Models;
using Proyecto_Final.ArmadoEstructuras;

namespace Proyecto_Final.Servicio
{
    public class TransaccionServicio

    {
        //hacer mas pruebas

        public ContextDatos ContextoEstructuras { get; set; }
        public int MAXIMO_RECIENTES;

        public TransaccionServicio(ContextDatos datos)
        {
            ContextoEstructuras = datos;
            MAXIMO_RECIENTES = 25;
        }

        public void NuevaTransaccion(Transaccion tx)
        {
            Tarjeta tarjeta = null;
            //verificar que la tarjeta exista, no este bloqueada o no este a cero
            foreach(var t in ContextoEstructuras.colaTarjetas.ObtenerTodo())
            {
                if (t.Numero == tx.Numero)
                {
                    tarjeta = t;
                    break;
                }
            }
          

            if (tarjeta == null) {
                return;
            }

            if (!tarjeta.IsBlocked || tarjeta.Balance > 0m)
            {

                // return;
                ContextoEstructuras.abbTransacciones.Insertar(tx);
                //Cola de Transacciones Pendientes
                ContextoEstructuras.ColaTransacciones.Encolar(tx);
            }
            
            



        }
        //solo es del ABB
        public IEnumerable<Transaccion> ObtenerTransaciones()
        {
            return ContextoEstructuras.abbTransacciones.ObtenerTodo();

        }
        //en conjunto con ContextoDatos ya tiene que estar vinculado Tarjeta-Transaccion
        public bool ProcesarTransaccion()
        {
            //nada q procesar
            if (ContextoEstructuras.ColaTransacciones.estaVacio())
            {
                return false;
            }

            //desencolar la transaccion
            Transaccion trx = ContextoEstructuras.ColaTransacciones.Desencolar();
            Tarjeta tarjetaEncontrada = null;
     
                foreach (var tarjeta in ContextoEstructuras.colaTarjetas.ObtenerTodo())
            {
                if (tarjeta.Numero == trx.Numero)
                {
                  
                    tarjetaEncontrada = tarjeta;
                    break;
                }

            }
            if (tarjetaEncontrada == null)
            {
                return false;
            }

            //si la tarjeta ya esta bloqueadaa no admite ninguna trans
            if (tarjetaEncontrada.IsBlocked)
            {
                //tarjetaEncontrada.ElimTransBuscar(trx);
                return true;
            }

            //ajuste del balance correspondiente a la tarjeta
            tarjetaEncontrada.Balance -= trx.Monto;//debitamos el gasto
            if (tarjetaEncontrada.Balance <= 0m) { 
                tarjetaEncontrada.Balance = 0m; 
                tarjetaEncontrada.IsBlocked = true;
            }

            //si la transaccion excede el monto de la tranjeta
            /*
            if (trx.Monto > tarjetaEncontrada.Balance) {
                tarjetaEncontrada.Balance = 0m;
                tarjetaEncontrada.IsBlocked = true;
                return true;
            }*/

            //agregamos la transaccion al histrial de las tarjetas
            bool EsaYaExiste = false;
            foreach (var t in tarjetaEncontrada.Transacciones) {
                if (t.Id == trx.Id) {
                    EsaYaExiste = true;
                    break;
                }
            }
            if (!EsaYaExiste) {
                tarjetaEncontrada.AgregarTransaccion(trx);
            }


            //agregamos la transaccion a la pila de recientes   
            ContextoEstructuras.pilaTransacciones.Push(trx);
            while (ContextoEstructuras.pilaTransacciones.ObtenerTodo().Count() > MAXIMO_RECIENTES) {
                ContextoEstructuras.pilaTransacciones.Pop();
            }
            return true;

        }



        public void ProcesarTodo()
        {
            //sigo hasta que se desencole toda la cola
            while (ProcesarTransaccion())
            {
                if (ContextoEstructuras.ColaTransacciones.estaVacio())
                {
                    break;
                }
            }
        }

        

        //Mostrar cola de Pendientes
        public IEnumerable<Transaccion> ObtenerPendientes_Cola()
        {
            return ContextoEstructuras.ColaTransacciones.ObtenerTodo();
        }

        //Mostrar pila historial recientes
        public IEnumerable<Transaccion> ObtenerRecientes_Pila(int n)
        {
            //obtenemos solo las n transacciones no mayores a 10 por peticion
            return ContextoEstructuras.pilaTransacciones.ObtenerTodo().Take(n);
        }




        //como un historial completo ABB
        public string BuscarTransaccion(string id)
        {

            return ContextoEstructuras.abbTransacciones.Buscar(id).ToString();
        }


    }
}
