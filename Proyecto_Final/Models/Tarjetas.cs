using System;
using System.Collections.Generic;
using System.Text;
using Proyecto_Final.Models;

namespace Models
{
    public class Tarjeta:AccesoId, AccesoTarjeta_NumeroTarjeta
    {
        public string  Id { get; set; }
        public string Numero { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public int Cvv { get; set; }
        public int Pin { get; set; }
        public decimal Balance { get; set; }
        public decimal LimiteCredito { get; set; }
        public bool IsBlocked { get; set; }
        public string IdCliente { get; set; }
        public List<Transaccion> Transacciones { get; set; } = new List<Transaccion>();


        public Tarjeta()
        {
            
        }

        public Tarjeta(string id,string numero, DateTime fechaExpiracion, int cvv,int pin,decimal balance, decimal limiteCredito, bool estaBloqueada, string idCliente)
        {
            Id = id;
            Numero = numero;
            FechaExpiracion = fechaExpiracion;
            Cvv = cvv;
            Pin = pin;  
            Balance = balance;
            LimiteCredito = limiteCredito;
            IsBlocked = estaBloqueada;
            IdCliente = idCliente;
            Transacciones = new List<Transaccion>();

        }

        public string VerTransacciones()
        {
            StringBuilder sb = new StringBuilder();
            if (Transacciones != null && Transacciones.Count > 0)
            {
                foreach (Transaccion item in Transacciones)
                {
                    sb.AppendLine(item.ToString());
                }
            }
            else
            {
                sb.AppendLine("No hay transacciones registradas.");
            }
            return sb.ToString();
        }

        public void AgregarTransaccion(Transaccion trx)
        {
            if (trx != null)
            {
                Transacciones.Add(trx);
            }
        }

        public override string ToString()
        {
            return $"\n\tId :{Id}"+
                   $"\n\tNumero: {Numero}," +
                   $"\n\tExpira el: {FechaExpiracion}," +
                   $"\n\tCVV: {Cvv}," +
                   $"\n\tPin: {Pin}," +
                   $"\n\tBalance: {Balance}," +
                   $"\n\tLímite de Crédito: {LimiteCredito}," +
                   $"\n\tEstá Bloqueada: {IsBlocked}," +
                   $"\n\tID Cliente Asociado: {IdCliente}" +
                   $"\n\tHistorial de Transacciones: [\n\t{VerTransacciones()}]";
        }
    }
}