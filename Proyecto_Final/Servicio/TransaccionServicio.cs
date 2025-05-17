using Models;
using Proyecto_Final.ArmadoEstructuras;

namespace Proyecto_Final.Servicio
{
    public class TransaccionServicio

    {
        /**No funciona  --quitar los desencolados y dejar solo el de procesarTransaccion*/

        public ContextDatos ContextoEstructuras { get; set; }
        public int MAXIMO_RECIENTES;

        public TransaccionServicio(ContextDatos datos)
        {
            ContextoEstructuras = datos;
            MAXIMO_RECIENTES = 15;
        }

        public void NuevaTransaccion(Transaccion tx)
        {
            ContextoEstructuras.abbTransacciones.Insertar(tx);
            //Cola de Transacciones Pendientes
            ContextoEstructuras.ColaTransacciones.Encolar(tx);

        }
        //solo es del ABB
        public IEnumerable<Transaccion> ObtenerTransaciones()
        {
            return ContextoEstructuras.abbTransacciones.ObtenerTodo();

        }
        //en conjunto con ContextoDatos ya tiene que estar vinculado Tarjeta-Transaccion
        public bool ProcesarTransaccion()
        {
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

            //ajuste del balance correspondiente a la tarjeta
            tarjetaEncontrada.Balance -= trx.Monto;//debitamos el gasto
            if (tarjetaEncontrada.Balance <= 0m) { 
                tarjetaEncontrada.Balance = 0m; 
                tarjetaEncontrada.IsBlocked = true;
            }

            //agregamos la transaccion al histrial de las tarjetas
            tarjetaEncontrada.AgregarTransaccion(trx);
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
