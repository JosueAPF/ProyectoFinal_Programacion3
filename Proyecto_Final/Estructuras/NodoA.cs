namespace Estructuras
{
    public class NodoA<T>
    {
        public T Dato { get; set; }
        public NodoA<T> Izq{get;set;}
        public NodoA<T> Der { get; set; }

 
        public NodoA(T dato)
        {
            this.Dato = dato;
            this.Izq = this.Der = null;
        }

        public override string ToString()
        {
            return $"{Dato}"; 
        }
    }
}
