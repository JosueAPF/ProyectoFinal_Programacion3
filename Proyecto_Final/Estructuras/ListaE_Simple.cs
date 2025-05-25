using System.Collections;
using System.Text;
using Models;
using Proyecto_Final.Models;

namespace Estructuras
{
    public class ListaE_Simple<T>  where T : AccesoId
    {
        public Nodo<T> Cabeza { get; set; }    
        public Nodo<T> Ultimo { get; set; }

     
        public ListaE_Simple()
        {
            Cabeza = null;
            Ultimo = null;
        }


        public void Enlistar(T dato) {
            Nodo<T> nuevo = new Nodo<T>(dato);
            if (Cabeza == null)
            {
                Cabeza = nuevo;
                Ultimo = nuevo;
            }
            else
            {
                Ultimo.Enlazar(nuevo);
                Ultimo = nuevo;
            }
        }
        public T Mostrar() {
            Nodo<T> auxCabeza = Cabeza;
            T dato = default(T);    
            while (auxCabeza != null)
            {
                dato = auxCabeza.Dato;
                auxCabeza = auxCabeza.Sig;
            }
            return dato;
        }
        
        public void EliminarTodo()
        {
            Cabeza = null;
            Ultimo = null;
        }   

        public T Eliminar(int id) {
            if (Cabeza == null) {
                return default(T);
            }
            //caso si es la cabeza
            if (Cabeza.Dato.Id.Equals(id)) {
                Nodo<T> NodoElim = Cabeza;
                Cabeza = Cabeza.Sig;
                NodoElim.Sig = null;
                return NodoElim.Dato;   
            }

            //caso si no es la cabeza
            Nodo<T> actual = Cabeza;
            Nodo<T> anteriror = null;

            while (actual.Sig != null && !actual.Dato.Id.Equals(id)) {
                anteriror = actual;
                actual = actual.Sig;
            }

            anteriror.Sig = actual.Sig;
            return actual.Dato; 
        }
        
        public string Bucar(int id) {
            StringBuilder sb = new StringBuilder(); 
            if (Cabeza == null) { 
                return "vacio";  
            }

            Nodo<T> actual = Cabeza;
            while (actual !=null) {
                if (actual.Dato.Id.Equals(id)) { 
                    sb.AppendLine(actual.ToString());   
                }
                actual = actual.Sig;    
            }
            return sb.ToString(); 
        }
        
        public bool EstaVacio()
        {
            return Cabeza == null;
        }   

        public IEnumerable<T>  ObtenerTodo()
        {
            Nodo<T> actual = Cabeza;
            while (actual != null)
            {
                yield return actual.Dato;
                actual = actual.Sig;
            }
        }
    }
}
