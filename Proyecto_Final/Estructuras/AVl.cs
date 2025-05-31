using Estructuras;
using Models;

namespace Proyecto_Final.Estructuras
{
    public class AVl<T> where T : AccesoId
    {
        public NodoA<T> Raiz { get; set; }

        /*Otener Altura de Nodo*/
        public int GetAltuar(NodoA<T> nodo)
        {
            if (nodo != null)
            {
                return nodo.Altura;
            }
            return 0;
        }
        /*Calcular el Factor de Equilibrio*/
        public int FalctoEquilibrio(NodoA<T> nodo)
        {
            if (nodo != null)
            {
                return GetAltuar(nodo.Der) - GetAltuar(nodo.Izq);
            }
            return 0;
        }

        /*Acutalizador de Altura*/
        public void ActualizarAltura(NodoA<T> nodo)
        {
            nodo.Altura = Math.Max(GetAltuar(nodo.Izq), GetAltuar(nodo.Der)) + 1; // 1 para evitar el cero:::  
        }

        /*****************************Rotaciones ********/

        /*(Izquierda-Izquierda), Rotacion Derecha*/
        public NodoA<T> RotacionDerecha(NodoA<T> nodo)
        {
            NodoA<T> aux1 = nodo.Izq;
            NodoA<T> aux2 = aux1.Der;

            aux1.Der = nodo;
            nodo.Izq = aux2;

            ActualizarAltura(nodo);
            ActualizarAltura(aux1);

            return aux1; //el Nuevo Nodo Raiz
        }

        /*(Derecha-Derecha), Rotacion Izquierda*/
        public NodoA<T> RotacionIzquierda(NodoA<T> nodo)
        {
            NodoA<T> aux1 = nodo.Der;
            NodoA<T> aux2 = aux1.Izq;

            aux1.Izq = nodo;
            nodo.Der = aux2;

            ActualizarAltura(nodo);
            ActualizarAltura(aux1);

            return aux1;

        }

        /*Metodos Recursivos*/
        public NodoA<T> InsercionRec(NodoA<T> nodo, T dato)
        {
            if (nodo == null)
            {
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


            // Rotación simple Izq-Izq
            if (Fe < -1 && dato.Id.CompareTo(nodo.Izq.Dato.Id) < 0)
                return RotacionDerecha(nodo);

            // Rotación simple Der-Der
            if (Fe > 2 && dato.Id.CompareTo(nodo.Der.Dato.Id) > 0)
                return RotacionIzquierda(nodo);

            // Rotación doble Izq-Der
            if (Fe < -1 && dato.Id.CompareTo(nodo.Izq.Dato.Id) > 0)
            {
                nodo.Izq = RotacionIzquierda(nodo.Izq);
                return RotacionDerecha(nodo);
            }

            // Rotación doble Der-Izq
            if (Fe > 2 && dato.Id.CompareTo(nodo.Der.Dato.Id) < 0)
            {
                nodo.Der = RotacionDerecha(nodo.Der);
                return RotacionIzquierda(nodo);
            }



            return nodo;
        }
        //eliminacion x Id
        public NodoA<T> ElimRec(NodoA<T> nodo, T dato)
        {
            if (nodo == null)
                return null;

            // 1) Busco el nodo según la clave (Id)
            if (dato.Id.CompareTo(nodo.Dato.Id) < 0)
            {
                // reasigno el hijo izquierdo con la subraíz resultante
                nodo.Izq = ElimRec(nodo.Izq, dato);
            }
            else if (dato.Id.CompareTo(nodo.Dato.Id) > 0)
            {
                // reasigno el hijo derecho con la subraíz resultante
                nodo.Der = ElimRec(nodo.Der, dato);
            }
            else
            {
                // 2) Si encuentro el nodo a eliminar:
                //    - Caso 1 y 2: tiene uno o ningún hijo
                if (nodo.Izq == null)
                {
                    return nodo.Der;  // devuelve el hijo derecho (o null si no tiene)
                }
                else if (nodo.Der == null)
                {
                    return nodo.Izq;  // devuelve el hijo izquierdo
                }

                //    - Caso 3: dos hijos → tomo el sucesor (mínimo en la rama derecha)
                NodoA<T> aux = ObtenerMinimo(nodo.Der);
                // Copio el dato del sucesor en el nodo actual
                nodo.Dato = aux.Dato;
                // Elimino recursivamente ese sucesor en la subrama derecha
                nodo.Der = ElimRec(nodo.Der, aux.Dato);
            }

            // 3) Actualizo la altura de este nodo “nodo”
            ActualizarAltura(nodo);

            // 4) Recalculo factor de equilibrio partiendo de tu método
            int Fe = FalctoEquilibrio(nodo);

            // (Recuerda que tu método FalctoEquilibrio devuelve altura(derecha) - altura(izquierda))

            // 5) Si el subárbol quedó desbalanceado, aplico rotaciones:
            //    - Fe < -1 → rama izquierda más alta de lo permitido
            if (Fe < -1 && FalctoEquilibrio(nodo.Izq) <= 0)
            {
                // **Rotación simple Izq-Izq**
                return RotacionDerecha(nodo);
            }
            if (Fe < -1 && FalctoEquilibrio(nodo.Izq) > 0)
            {
                // **Rotación doble Izq-Der**
                nodo.Izq = RotacionIzquierda(nodo.Izq);
                return RotacionDerecha(nodo);
            }

            //    - Fe > 2 → rama derecha más alta de lo permitido
            if (Fe > 2 && FalctoEquilibrio(nodo.Der) >= 0)
            {
                // **Rotación simple Der-Der**
                return RotacionIzquierda(nodo);
            }
            if (Fe > 2 && FalctoEquilibrio(nodo.Der) < 0)
            {
                // **Rotación doble Der-Izq**
                nodo.Der = RotacionDerecha(nodo.Der);
                return RotacionIzquierda(nodo);
            }

            // 6) Si ya no hace falta rotar, simplemente devuelvo este nodo
            return nodo;
        }

        private NodoA<T> ObtenerMinimo(NodoA<T> nodo)
        {
            while (nodo.Izq != null)
            {
                nodo = nodo.Izq;
            }
            return nodo;

        }

        public NodoA<T> BuscarRec(NodoA<T> nodo, string dato)
        {

            if (nodo == null)
            {
                return null;
            }

            if (dato.CompareTo(nodo.Dato.Id) < 0)
            {
                return BuscarRec(nodo.Izq, dato);
            }
            else if (dato.CompareTo(nodo.Dato.Id) > 0)
            {
                return BuscarRec(nodo.Der, dato);
            }

            return nodo;
        }


        /*Metodos que consumen Metodos Recursivos*/
        public void Insertar(T dato)
        {
            Raiz = InsercionRec(Raiz, dato);
        }
        public void Eliminar(T dato)
        {
            Raiz = ElimRec(Raiz, dato);
        }
        public void Buscar(string dato)
        {
            BuscarRec(Raiz, dato);
        }






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
