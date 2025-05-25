using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estructuras;
using Models;

namespace TablaHash_E
{
    public class TablaHash<T> where T : AccesoId
    {

        public static readonly int M = 17;

        ListaEnlazada<T>[] Tabla;
        int Posicion = 1;
        public TablaHash()
        {
            Tabla = new ListaEnlazada<T>[M];

            //inicializa cada posicion con una  tabla vacia
            for (int i = 0; i < M; i++)
            {
                Tabla[i] = new ListaEnlazada<T>();
            }
        }
        private int DispersionMod(int Clave)
        {
            return Clave % M;
        }
        private int Convertir_Int(string Clave)
        {
            return Math.Abs(Clave.GetHashCode());

        }
        public int CrearPosicion(string clave)
        {
            int claveInt = Convertir_Int(clave);
            return DispersionMod(claveInt);
        }

        public bool Insertar(T dato)
        {

            Posicion = CrearPosicion(dato.Id);

            if (Tabla[Posicion].siExiste(dato.Id))
            {
                return false;
            }

            Tabla[Posicion].InsertarCabeza(dato);
            Console.WriteLine($"\tNueva Insercion: {dato}");
            MostrarTabla();
            Console.WriteLine("\n");
            return true;
        }

        public void MostrarTabla()
        {
            for (int i = 0; i < Tabla.Length; i++)
            {
                if (Tabla[i] == null)
                {
                    Console.WriteLine($"Posicion [{i}] : Vacio");
                }
                else
                {
                    Console.WriteLine($"Posicion [{i}] : {Tabla[i].MuestraLista()}");

                }

            }

        }
        public Nodo<T> BuscarTabla(string clave)
        {
            int Posicion = CrearPosicion(clave);
            return Tabla[Posicion].BuscarNodo(clave);

        }

        public string Eliminar(string clave)
        {
            int Posicion = CrearPosicion(clave);

            if (Tabla[Posicion].siExiste(clave))
            {
                Tabla[Posicion].Eliminar(clave);
                return $"Estaba en {Posicion}, Eliminado.. ";
            }
            return "No existe esa Clav";
        }

        public string Modificar(string clave, T nuevoDato) {
            int Posicion = CrearPosicion(clave);
            if (Tabla[Posicion].siExiste(clave)) {
                Tabla[Posicion].Modificar(clave, nuevoDato);
                return $"{nuevoDato}, Modificado con Exito";
            }
            return "Esa clave no Existe";
        }

        public IEnumerable<T> ObtenerTodo()
        {
            List<T> resultado = new List<T>();
            for (int i = 0; i < Tabla.Length; i++)
            {
                if (Tabla[i] != null)
                {
                    resultado.AddRange(Tabla[i].ObtenerTodo());
                }
            }
            return resultado;

        }
    }
}
