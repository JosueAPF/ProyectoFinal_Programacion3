using System;
using System.Text;
using Models;
using Proyecto_Final.Models;
namespace Estructuras
{
    public class Cola<T> where T : AccesoId
    {
        public Nodo<T> Primero { get; set; }    
        public Nodo<T> Ultimo { get; set; } 

        public Cola()
        {
            Primero = null;
            Ultimo = null;
        }

        public void Encolar(T dato) {
            Nodo<T> nuevo = new Nodo<T>(dato);
            if (Primero == null)
            {
                Primero = nuevo;
                Ultimo = nuevo;
            }
            else { 
                Ultimo.Enlazar(nuevo);
                Ultimo = nuevo;
            }
        }
        public string Mostrar() {
            StringBuilder sb = new StringBuilder();
            Nodo<T> auxPrimero = Primero;
            while (auxPrimero != null) { 
                sb.AppendLine(auxPrimero.ToString());  
                auxPrimero = auxPrimero.Sig;
            }
            return sb.ToString();
        }
        public T Desencolar() {
            if (Primero == null) { 
                return default(T);  
            }

            Nodo<T> ElimNodo = Primero;
            Primero = Primero.Sig;
            if (Primero == null) {
                Ultimo = null;
            }

            return ElimNodo.Dato;   

        }
        /*Buscar x Id*/
        public string Buscar(string id) {
            StringBuilder sb = new StringBuilder();
            Nodo<T> auxPrimero = Primero;
            while (auxPrimero != null)
            {
                if (auxPrimero.Dato.Id.Equals(id)) { 
                    sb.AppendLine(auxPrimero.ToString());
                }
                auxPrimero = auxPrimero.Sig;
            }
            return sb.ToString();

        }

        

        /*Modificar */
        public string Modificar(string id, T nuevoDato)
        {
            StringBuilder sb = new StringBuilder();
            Nodo<T> auxPrimero = Primero;
            while (auxPrimero != null)
            {
                if (auxPrimero.Dato.Id.Equals(id))
                {
                    auxPrimero.Dato = nuevoDato;
                    sb.AppendLine(auxPrimero.ToString());
                }
                auxPrimero = auxPrimero.Sig;
            }
            return sb.ToString();
        }


        public bool estaVacio() {
            return Primero == null;
        }

        public T Desencolar_TipoLista(string id) {
            if (Primero == null)
            {
                return default(T);
            }
            //caso si es la cabeza
            if (Primero.Dato.Id.Equals(id))
            {
                Nodo<T> NodoElim = Primero;
                Primero = Primero.Sig;
                NodoElim.Sig = null;
                return NodoElim.Dato;
            }

            //caso si no es la cabeza
            Nodo<T> actual = Primero;
            Nodo<T> anteriror = null;

            while (actual.Sig != null && !actual.Dato.Id.Equals(id))
            {
                anteriror = actual;
                actual = actual.Sig;
            }

            anteriror.Sig = actual.Sig;
            return actual.Dato;
        }

        public IEnumerable<T> ObtenerTodo()
        {
            Nodo<T> actual = Primero;
            while (actual != null)
            {
                yield return actual.Dato;
                actual = actual.Sig;
            }
        }

    }
}
