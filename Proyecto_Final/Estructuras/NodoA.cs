namespace Estructuras
{
    public class NodoA<T>
    {
        public T Dato { get; set; }
        public NodoA<T> Izq{get;set;}
        public NodoA<T> Der { get; set; }
        public int Altura { get; set; } //uso exclusivo para AVL    

        public NodoA(T dato)
        {
            Dato = dato;
            Izq = null;
            Der = null;
            Altura = 1;
        }

        public override string ToString()
        {
            return $"{Dato}"; 
        }
    }
}
