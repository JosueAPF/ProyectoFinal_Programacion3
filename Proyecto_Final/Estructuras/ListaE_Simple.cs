using System.Collections;
using System.Text;
using Models;

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
        public string Mostrar() {
            StringBuilder sb = new StringBuilder();
            Nodo<T> auxCabeza = Cabeza;
            while (auxCabeza != null)
            {
                sb.AppendLine(auxCabeza.ToString());
                auxCabeza = auxCabeza.Sig;
            }
            return sb.ToString();
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

     

        public IEnumerator<T> GetEnumerator()
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
