using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public class Lista
    {
        public string Tipo { get; set; }
        public List<Tipo_Collection> Lista_valores { get; set; }

        public Lista(string tipo)
        {
            this.Tipo = tipo;
            this.Lista_valores = new List<Tipo_Collection>();
        }

        public void Insert(Tipo_Collection valor)
        {
            Lista_valores.Add(valor);
        }
        public object Get(int posicion)
        {

            try
            {
                return Lista_valores.ElementAt(posicion);
            }
            catch { return null; }
        }
        public bool Set(int posicion, Tipo_Collection valor)
        {

            try
            {
                Lista_valores[posicion] = valor;
                return true;
            }
            catch { return false; }
        }
        public bool Remove(int posicion) {
            try
            {
                Lista_valores.RemoveAt(posicion);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public int Size()
        {

            return Lista_valores.Count();
        }
        public void Clear() { Lista_valores.Clear(); }

        public bool Contains(object valor) {
            return Lista_valores.Contains(valor);
        }
    }
}
