

using Models;

namespace Estructuras
{
    public class ABB<T> where T : AccesoId
    {
        public NodoA<T> Raiz { get; set; }  
        public ABB()
        {
            Raiz = null;
        }


        //Metodos Recursivos
        public NodoA<T> InsetarRec(NodoA<T> nodo, T dato) {
            if (nodo == null) { 
                return new NodoA<T>(dato);
            }

            if (dato.Id.CompareTo(nodo.Dato.Id) < 0)
            {
                nodo.Izq = InsetarRec(nodo.Izq, dato);
            }
            else if (dato.Id.CompareTo(nodo.Dato.Id) > 0) { 
                nodo.Der = InsetarRec(nodo.Der, dato);  
            }

            return nodo;

        }

        public NodoA<T> Eliminar(NodoA<T> nodo, T dato) {
            if (nodo == null) {
                return null;
            }

            if (dato.Id.CompareTo(nodo.Dato.Id) < 0)
            {
                nodo.Izq = Eliminar(nodo.Izq, dato);
            }
            else if (dato.Id.CompareTo(nodo.Dato.Id) > 0)
            {
                nodo.Der = Eliminar(nodo.Der, dato);
            }
            else
            {
                // Caso 1: Nodo sin hijos
                if (nodo.Izq == null && nodo.Der == null)
                {
                    return null;
                }
                // Caso 2: Nodo con un hijo
                else if (nodo.Izq == null)
                {
                    return nodo.Der;
                }
                else if (nodo.Der == null)
                {
                    return nodo.Izq;
                }
                // Caso 3: Nodo con dos hijos

                NodoA<T> menor = ObtenerMenor(nodo.Der);
                nodo.Dato = menor.Dato;
                nodo.Der = Eliminar(nodo.Der, menor.Dato);  

            }
            return nodo;
        }

        public NodoA<T> ObtenerMenor(NodoA<T> nodo)
        {
            if (nodo.Izq == null)
            {
                return nodo;
            }
            return ObtenerMenor(nodo.Izq);
        }   


        //Metodos que consumen Metodos Recursivos
    }
}
