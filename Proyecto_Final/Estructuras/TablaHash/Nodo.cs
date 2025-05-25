using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TablaHash_E
{
    public class Nodo<T>
    {
        public T Dato { get; set; }
        public Nodo<T> Enlaze { get; set; }

        public Nodo(T dato)
        {
            Dato = dato;
            Enlaze = null;
        }
        
        public override string ToString()
        {
            return Dato.ToString();
        }
    }
}
