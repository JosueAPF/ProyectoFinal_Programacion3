using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace TablaHash_E
{
    public class ListaEnlazada<T> where T:AccesoId
    {

        public Nodo<T> primero;
        public ListaEnlazada()
        {
            primero = null;
        }

        public ListaEnlazada<T> InsertarCabeza(T dato) {
            if (siExiste(dato.Id)) {
                return this;
            }

            Nodo<T> nuevo = new Nodo<T>(dato);
            nuevo.Enlaze = primero;  
            primero = nuevo;    
            return this;    
        }

        public Nodo<T> BuscarNodo(string id)
        {
            Nodo<T> temp = primero;
            while (temp != null )
            {
                if (temp.Dato.Id.Equals(id)) { 
                    return temp;
                }
                temp = temp.Enlaze;
            }
           return null;   
        }
        
       

        public bool siExiste(string id) {
            Nodo<T> temp = primero;

            while (temp != null)
            {
                if (temp.Dato.Id.Equals(id)) { 
                    return true;
                
                }
               temp = temp.Enlaze;
            }
           

            return false;
        }

        public String MuestraLista()
        {
            Nodo<T> auxP = primero;
            String resultado = "";
            while (auxP != null)
            {
                resultado = resultado + auxP.Dato + " -> ";
                auxP = auxP.Enlaze;
            }
            return resultado;
        }   

        public void Eliminar(string id)
        {
            //caso de si la lista esta vacia
            if (primero == null) {
                return;
            }

            //caso de si el nodo a eliminar es el primero
            if (primero.Dato.Id.Equals(id)) { 
                primero = primero.Enlaze;
                return;
            }

            //calquier otro q no sea el primer nodo

            Nodo<T> temp = primero;
            Nodo<T> anterior = null;

            while (temp.Enlaze != null && !temp.Dato.Id.Equals(id)) {
                anterior = temp;
                temp = temp.Enlaze;
            }
            anterior.Enlaze = temp.Enlaze;  

        }

        public string Modificar(string anteriror, T datonuevo) {
            Nodo<T> clienteBuscar = BuscarNodo(anteriror);
            if (clienteBuscar == null) {
                return "Ese Cliente no existe!";
            }

            Eliminar(clienteBuscar.Dato.Id);
            InsertarCabeza(datonuevo);
            return "Modificado con Existo!";

        }

        public IEnumerable<T> ObtenerTodo()
        {
            Nodo<T> actual = primero;
            while (actual != null)
            {
                yield return actual.Dato;
                actual = actual.Enlaze;
            }
        }
    }
}
