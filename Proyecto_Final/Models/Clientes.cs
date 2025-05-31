using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estructuras;
using Proyecto_Final.Models;

namespace Models
{
    public class Clientes: AccesoId
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string DPI { get; set; }
        public List<Tarjeta> tarjetas { get; set; } = new List<Tarjeta>();
        //public ListaE_Simple<Tarjeta> tarjetas { get; set; } = new ListaE_Simple<Tarjeta>();   



        //Constructor Vacio para la deserealizacion
        public Clientes()
        {

        }
        //constructor con parametros
        public Clientes(string id, string name,string dpi)
        {
            this.Id = id;
            this.Name = name;
            this.DPI = dpi;
            //tarjetas = new ListaE_Simple<Tarjeta>();    
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

        public void EliminarTarjeta() {
            if (tarjetas == null) {

                return;
            }

            tarjetas.Clear();
        }


        public override string ToString()
        {
            return $"\nId :{Id}," +
                $"\nNombre :{Name}, " +
                $"\nDPI :{DPI}, " + 
                $"\nTarjetas Asociadas :  [{VerTarjetas()}], ";
                
        }


    }

}

