﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Proyecto_Final.ArmadoEstructuras;

namespace Proyecto_Final.Servicio
{
    public class TransaccionServicio

    {

        public ContextDatos ContextoEstructuras { get; set; }
        public int MAXIMO_RECIENTES;
        

        public TransaccionServicio(ContextDatos datos)
        {
            ContextoEstructuras = datos;
            MAXIMO_RECIENTES = 35;
        }


        public bool RealizarPago(Transaccion tx)
        {
            //arbol Historial y busqueda 
            ContextoEstructuras.abbTransacciones.Insertar(tx);
            //Cola de Transacciones Pendientes
            ContextoEstructuras.ColaTransacciones.Encolar(tx);

            return true;
        }

        public bool RealizarCompra(Transaccion tx)
        {
            ContextoEstructuras.abbTransacciones.Insertar(tx);
            ContextoEstructuras.ColaTransacciones.Encolar(tx);

            return true;
        }

        public void NuevaTransaccion(Transaccion tx)
        {
            
            ContextoEstructuras.abbTransacciones.Insertar(tx);
            //Cola de Transacciones Pendientes
            ContextoEstructuras.ColaTransacciones.Encolar(tx);
        }
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
                if (tarjeta.Numero == trx.NueroTarjeta)
                {

                    tarjetaEncontrada = tarjeta;
                    break;
                }

            }
            if (tarjetaEncontrada == null)
            {
                return true;
            }


            /******************Tipo de Transcciones******************/


            /**pago**/
            if (trx.Tipo == TipoTransaccion.Pago)
            {
                if (trx.Monto < 0) {
                    return false;
                }

                if (tarjetaEncontrada.deuda == 0)
                    return false; // Nada que pagar

                // mejor
                if (trx.Monto > 0 && trx.Monto <= tarjetaEncontrada.deuda)
                {
                   tarjetaEncontrada.deuda -= trx.Monto;
                   
                }


                if (tarjetaEncontrada.IsBlocked && tarjetaEncontrada.SaldoDisponible() > 0m) { 
                    tarjetaEncontrada.IsBlocked = false;
                }
            }


            /**Compra**/
            if (trx.Tipo == TipoTransaccion.Compra)
            {
                if (tarjetaEncontrada.IsBlocked)
                {

                    return true;
                }

                decimal NuevaDeuda = tarjetaEncontrada.deuda + trx.Monto;
                if (NuevaDeuda >= tarjetaEncontrada.LimiteCredito)
                {
                    tarjetaEncontrada.deuda = tarjetaEncontrada.LimiteCredito;
                    tarjetaEncontrada.IsBlocked = true;
                }
                else { 
                    tarjetaEncontrada.deuda += trx.Monto;
                
                }

            }
            //agregamos la transaccion al histrial de las tarjetas
            bool EsaYaExiste = false;
            foreach (var t in tarjetaEncontrada.Transacciones)
            {
                if (t.Id == trx.Id)
                {
                    EsaYaExiste = true;
                    break;
                }
            }
            if (!EsaYaExiste)
            {
                tarjetaEncontrada.AgregarTransaccion(trx);
            }


            //agregamos la transaccion a la pila de recientes   
            ContextoEstructuras.pilaTransacciones.Push(trx);
            while (ContextoEstructuras.pilaTransacciones.ObtenerTodo().Count() > MAXIMO_RECIENTES)
            {
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
            //obtenemos solo las n transacciones no mayores a n por peticion
            return ContextoEstructuras.pilaTransacciones.ObtenerTodo().Take(n);
        }




        //como un historial completo ABB
        public string BuscarTransaccion(string id)
        {

            return ContextoEstructuras.abbTransacciones.Buscar(id).ToString();
        }


    }
}
