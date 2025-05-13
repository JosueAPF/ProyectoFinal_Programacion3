using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_Final.Models;

namespace Models
{
    public class Clientes: AccesoId, AcceosTarjetas, Acceoso_AddTarjeta
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public List<Tarjeta> tarjetas { get; set; } = new List<Tarjeta>();
       


        //Constructor Vacio para la deserealizacion
        public Clientes()
        {

        }
        //constructor con parametros
        public Clientes(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public String VerTarjetas() {
            StringBuilder sb = new StringBuilder();
            if (tarjetas != null) {
                foreach (Tarjeta item in tarjetas) {
                    sb.AppendLine(item.ToString());
                }
            }
            return sb.ToString();   
        }

        public void AgregarTarjeta(Tarjeta tarjeta)
        {
            if (tarjeta != null)
            {
                tarjetas.Add(tarjeta);
            }
        }   


        public override string ToString()
        {
            return $"\nId :{Id}," +
                $"\nNombre :{Name}, " +
                $"\nTarjetas Asociadas :  [{VerTarjetas()}], ";
                
        }


    }

}

