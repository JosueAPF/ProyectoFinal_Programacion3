﻿using Estructuras;
using Models;
using Proyecto_Final.ArmadoEstructuras;
using Proyecto_Final.Models;

namespace Proyecto_Final.Servicio
{

    public class TarjetaServicio
    {
        public ContextDatos ContextoEstructuras { get; set; }
        public Cola<CambioLimiteTarjeta> HistorialLimites { get; set; }
        public TarjetaServicio(ContextDatos contexto)
        {
            ContextoEstructuras = contexto;
            HistorialLimites = new Cola<CambioLimiteTarjeta>();
        }

        /*Metodos CRUD*/

        public IEnumerable<Tarjeta> ObtenerTarjetas()
        {
            return ContextoEstructuras.colaTarjetas.ObtenerTodo();

        }

        public void AgregarTarjeta(Tarjeta tarjeta)
        {
            ContextoEstructuras.colaTarjetas.Encolar(tarjeta);
        }



        /*Busqueda Por Id*/
        public string BuscarTarjetaxId(string id)
        {
            return ContextoEstructuras.colaTarjetas.Buscar(id);
        }

        /*Busqueda por Numero de Tarjeta*/
        public Tarjeta BuscarTarjetaxNumero(string nume)
        {
            var tarjetas = ContextoEstructuras.colaTarjetas.ObtenerTodo();
            Tarjeta tarjeta = null;
            foreach (var item in tarjetas)
            {
                if (item.Numero == nume)
                {
                    tarjeta = item;
                }
            }
            return tarjeta;
        }

        //corregido ver Saldo Disponible
        public string SaldoDisponible(string numTarjeta)
        {
            var tarjeta = BuscarTarjetaxNumero(numTarjeta);
            if (tarjeta == null)
            {
                return "Tarjeta no encontrada";
            }
            if (tarjeta.IsBlocked)
            {
                return "Esta Tarjeta esta Bloqueada";
            }
            return $"Limite de Credito :Q{tarjeta.LimiteCredito} - Deuda : Q{tarjeta.Balance}  Saldo Disponible = Q{tarjeta.SaldoDisponible()}";
        }


        public string AumentoLimite(string numeroTarjeta, decimal nuevoLimite)
        {
            var tarjeta = BuscarTarjetaxNumero(numeroTarjeta);
            decimal LimitesPermitidos = tarjeta.LimiteCredito * 2m; //no mayores a 2 limites de credito

            if (tarjeta == null)
            {
                return "Tarjeta No Encontrada!";
            }


            if (nuevoLimite < tarjeta.LimiteCredito)
            {
                return $"Este Limite {nuevoLimite}, no puede ser menor al limite anteriro {tarjeta.LimiteCredito}";
            }

            if (tarjeta.IsBlocked == true && tarjeta.Balance == 0m)
            {
                return $"La tarjeta se encuentra Bloqueada";
            }

            if (nuevoLimite > LimitesPermitidos)
            {
                return $"El nuevo limite no pude exceder los Limites Permitidos";
            }

            if (nuevoLimite < tarjeta.Balance)
            {
                return $"El nuevo Limite no puede ser menor a la deuda actual {tarjeta.Balance}";
            }

            //creacion del registro Cola<>
            var cambio_agregarLimite = new CambioLimiteTarjeta(tarjeta.IdCliente, DateTime.UtcNow, nuevoLimite, tarjeta.LimiteCredito);


            //guardamos en el historial
            HistorialLimites.Encolar(cambio_agregarLimite);

            tarjeta.LimiteCredito = nuevoLimite;

            return "Limite de Credito Actualizado!";
        }



       
        /*Prueba #1 cambio de pin**/
        public string CabioPin(string numtarjeta, int nuevoPin)
        {
            int CambiosRealizados = 0;
            var tarjeta = BuscarTarjetaxNumero(numtarjeta);

            if (tarjeta == null)
            {
                return $"tarjeta {numtarjeta} no Existe";
            }

            tarjeta.Pin = nuevoPin;
            CambiosRealizados++;


            return $"Tarjeta Id :{tarjeta.Id}, Numero Tarjeta: {tarjeta.Numero}, Nuevo Pin {nuevoPin}";
        }

        public string BloquearTarjeta(string numtarjeta)
        {
            var tarjeta = BuscarTarjetaxNumero(numtarjeta);
            if (tarjeta == null)
            {
                return $"tarjeta {numtarjeta} no Existe";
            }
            tarjeta.IsBlocked = true;
            return $"Tarjeta Id :{tarjeta.Id}, Numero Tarjeta: {tarjeta.Numero}, Esta Bloqueada";
        }

        public string DesbloquearTarjeta(string numtarjeta)
        {
            var tarjeta = BuscarTarjetaxNumero(numtarjeta);
            if (tarjeta == null)
            {
                return $"tarjeta {numtarjeta} no Existe";
            }
            tarjeta.IsBlocked = false;
            return $"Tarjeta Id :{tarjeta.Id}, Numero Tarjeta: {tarjeta.Numero}, Esta Desbloqueada";
        }

        public decimal verDeuda(string numtarjeta) {
            var tarjeta = BuscarTarjetaxNumero(numtarjeta);
            if (tarjeta == null)
            {
                return 0m;
            }
            return tarjeta.Balance;
        }


        /*Historial de Cambio de aumento de limite de una tarjeta*/
        public IEnumerable<CambioLimiteTarjeta>? verCambios_LimiteCredito(string idTarjeta)
        {
            if (HistorialLimites.estaVacio())
            {
                return Enumerable.Empty<CambioLimiteTarjeta>();
            }
            Tarjeta tarjeta = BuscarTarjetaxNumero(idTarjeta);
            if (tarjeta == null)
            {
                return Enumerable.Empty<CambioLimiteTarjeta>();
            }

            return HistorialLimites.ObtenerTodo();
        }
    }
}
