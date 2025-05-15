using Estructuras;
using Models;

namespace Proyecto_Final.Estructuras
{
    public class AVl<T>  where T : AccesoId
    {
        public NodoA<T> Raiz { get; set; }

        /*Otener Altura de Nodo*/
        public int GetAltuar(NodoA<T> nodo) {
            if (nodo != null) {
                return nodo.Altura;
            }
            return 0;
        }
        /*Calcular el Factor de Equilibrio*/
        public int FalctoEquilibrio(NodoA<T> nodo) {
            if(nodo != null) {
                return GetAltuar(nodo.Der) - GetAltuar(nodo.Izq);
            }
            return 0;
        }

        /*Acutalizador de Altura*/
        public void ActualizarAltura(NodoA<T> nodo) { 
            nodo.Altura = Math.Max(GetAltuar(nodo.Izq), GetAltuar(nodo.Der)) + 1; // 1 para evitar el cero:::  
        }

        /*****************************Rotaciones ********/

        /*(Izquierda-Izquierda), Rotacion Derecha*/
        public NodoA<T> RotacionDerecha(NodoA<T> nodo) { 
            NodoA<T> aux1 = nodo.Izq;
            NodoA<T> aux2 = aux1.Der;

            aux1.Der = nodo;    
            nodo.Izq = aux2;

            ActualizarAltura(nodo);
            ActualizarAltura(aux1);

            return aux1; //el Nuevo Nodo Raiz
        }

        /*(Derecha-Derecha), Rotacion Izquierda*/
        public NodoA<T> RotacionIzquierda(NodoA<T> nodo) { 
            NodoA<T> aux1 = nodo.Der;
            NodoA<T> aux2 = aux1.Izq;   

            aux1.Izq = nodo;
            nodo.Der = aux2;    

            ActualizarAltura(nodo);
            ActualizarAltura(aux1);

            return aux1;

        }

        /*Metodos Recursivos*/
        public NodoA<T> InsercionRec(NodoA<T> nodo, T dato) {
            if (nodo == null) { 
                return new NodoA<T>(dato);  
            }

            if (dato.Id.CompareTo(nodo.Dato.Id) < 0)
            {
                nodo.Izq = InsercionRec(nodo.Izq, dato);
            }
            else if (dato.Id.CompareTo(nodo.Dato.Id) > 0)
            {
                nodo.Der = InsercionRec(nodo.Der, dato);
            }
            /*Casos de Rotaciones */

            //#1 Actualizo el Nodo entrante
            ActualizarAltura(nodo);

            //#2 Calculo el Factor de Equilibrio lo gurado en una variable
            int Fe = FalctoEquilibrio(nodo);
            
            //*Rotaciones simples/

            //caso izquierdo izquierdo (Rotacion derecha)
            if (Fe < -1 && dato.Id.CompareTo(nodo.Izq.Dato) <=0) {
                return RotacionDerecha(nodo);
            }
            //caso derecho derecho (Rotacion izquierda)
            if (Fe > 2 && dato.Id.CompareTo(nodo.Der.Dato)>=0) {
                return RotacionIzquierda(nodo);
            }

            //casos dobles

            //caso Izquierdo-Derecho
            if (Fe < -1 && dato.Id.CompareTo(nodo.Izq.Dato) > 0) {
                nodo.Izq = RotacionIzquierda(nodo.Izq);
                return RotacionDerecha(nodo);
            }

            //caso Derecho-Izquierdo
            if (Fe > 2 && dato.Id.CompareTo(nodo.Der.Dato) > 0) {
                nodo.Der = RotacionDerecha(nodo.Der);
                return RotacionIzquierda(nodo);
            }

            return nodo;
        }
        //eliminacion x Id
        public NodoA<T> ElimRec(NodoA<T> nodo , T dato) {
            if (nodo == null) {
                return null;
            }

            if (dato.Id.CompareTo(nodo.Dato.Id) < 0)
            {
                ElimRec(nodo.Izq, dato);
            }
            else if (dato.Id.CompareTo(nodo.Dato.Id) > 0)
            {
                ElimRec(nodo.Der, dato);
            }
            else {
                //caso #1 y caso #2 un solo hijo
                if (nodo.Izq == null) {
                    return nodo.Der;
                } else if (nodo.Der == null) {
                    return nodo.Izq;
                }

                //caso 2 hijos

                //buscando el succesor mas ala izq
                var aux = ObtenerMinimo(nodo.Der);
                nodo.Der = ElimRec(nodo,aux.Dato);
            }
            ActualizarAltura(nodo);

            return nodo ;
        }
        private NodoA<T> ObtenerMinimo(NodoA<T> nodo) {
            while (nodo.Izq !=null) {
                nodo = nodo.Izq;
            }
            return nodo;
        
        }

        /*Metodos que consumen Metodos Recursivos*/


        ///ienumerable preorden
        public IEnumerable<T> ObtenerTodo()
        {
            return InOrder(Raiz);
        }

        private IEnumerable<T> InOrder(NodoA<T> nodo)
        {
            if (nodo == null)
                yield break;

            // subárbol izquierdo
            foreach (var x in InOrder(nodo.Izq))
                yield return x;

            // nodo actual
            yield return nodo.Dato;

            // subárbol derecho
            foreach (var x in InOrder(nodo.Der))
                yield return x;
        }
    }
}
