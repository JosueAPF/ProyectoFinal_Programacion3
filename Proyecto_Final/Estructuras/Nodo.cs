namespace Estructuras
{
    public class Nodo<T>
    {
        public T Dato { get; set; }
        public Nodo<T> Sig { get; set; }
        
        public Nodo<T> Ant { get; set; }//uso exclusivo para la lista doble enlazada

        public Nodo(T dato)
        {
            this.Dato = dato;
            this.Sig = null;
            this.Ant = null;
        }
        public Nodo<T> Enlazar(Nodo<T> nodo) { 
            this.Sig = nodo;
            return nodo;    
        }

        //solo para la lista enlazada doble
        public Nodo<T> EnlazadoDoble(Nodo<T> nodo) { 
            this.Sig = nodo;
            nodo.Ant = this;
            return nodo;
        }
        public override string ToString()
        {
            return $"{Dato}";
        }
    }
}
