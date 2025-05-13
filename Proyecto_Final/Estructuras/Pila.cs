using System.Text;
using Models;
using Proyecto_Final.Models;

namespace Estructuras
{
    public class Pila<T> where T : AccesoId
    {

        public Nodo<T> Tope { get; set; }
        public Pila()
        {
            Tope = null;
        }   

        public void Push(T dato)
        {
            Nodo<T> nuevo = new Nodo<T>(dato);
            nuevo.Sig = Tope;
            Tope = nuevo;   
        }
        public string Mostrar() {
            StringBuilder sb = new StringBuilder(); 
            Nodo<T> auxTope = Tope;
            while (auxTope != null) {
                sb.AppendLine(auxTope.ToString());   
                auxTope = auxTope.Sig;  
            }
            return sb.ToString();
        }

        public T Pop() { 
            StringBuilder sb = new StringBuilder();

            if (Tope == null) {
                return default(T);
            }

            Nodo<T> NodoElim = Tope;
            Tope = Tope.Sig;
            /*desenlaze del nodo eliminado*/
            NodoElim.Sig = null;    
            return NodoElim.Dato;   
        }

        /**Unicamente para pruebas de CRUD**/
        public string Buscar(string id) {
            StringBuilder sb = new StringBuilder();
            Nodo<T> auxTope = Tope;
            while (auxTope != null)
            {
                if (auxTope.Dato.Id == id) { 
                 sb.AppendLine(auxTope.ToString());    
                }
                auxTope = auxTope.Sig;
            }
            return sb.ToString();
        }

        public IEnumerable<T> ObtenerTodo()
        {
            var actual = Tope;
            while (actual != null)
            {
                yield return actual.Dato;
                actual = actual.Sig;
            }
        }


        /*
                IEnumerable permitiendo recorrer los elementos de tu Pila mediante . 
                Esto significa que puedes iterar sobre la estructura sin necesidad de 
                crear una lista intermedia, 
                lo cual es eficiente en términos de memoria.

                
                https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1?view=net-9.0

         */

    }
}
