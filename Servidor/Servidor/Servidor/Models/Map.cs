using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public class Map
    {
        public List<Item_Map> Mapita { get; set; }
        public string Clave { get; set; }
        public string Valor { get; set; }

        public Map(string clave, string valor)
        {
            this.Clave = clave;
            this.Valor = valor;
            Mapita = new List<Item_Map>();
        }
        public bool comprobarTipo() {
            bool is_ok = true;
            foreach (Item_Map item in Mapita)
            {
                if (!Program.casteos.comprobarCasteo(Program.getTipo(Clave), item.Clave)) is_ok = false;
            }

            return is_ok;
        }
        public bool containsKey(string clave)
        {
            foreach (Item_Map item in Mapita)
            {
                if (item.Clave.ToLower()==clave.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        public bool insert(string clave, object valor)
        {
            if (!containsKey(clave))
            {
                Mapita.Add(new Item_Map(clave, valor));
                return true;
            }
            return false;
        }
        public object Get(string clave)
        {
            foreach (Item_Map item in Mapita)
            {
                if (item.Clave.ToLower().Equals(clave.ToLower()))
                {
                    return item.Valor;
                }
            }
            return null;
        }
        public bool Set(string clave, object valor)
        {
            foreach (Item_Map item in Mapita)
            {
                if (item.Clave.ToLower().Equals(clave.ToLower()))
                {
                    item.Valor = valor;
                    return true;
                }
            }
            return false;
        }
        public bool Remove(string clave)
        {
            int index = -1;
            for (int i = 0; i < Mapita.Count; i++)
            {
                if (Mapita.ElementAt(i).Clave.ToLower().Equals(clave.ToLower())) index = i;
            }
            if (index != -1) { Mapita.RemoveAt(index); return true; }
            return false;
        }
        public int Size() {
            return Mapita.Count;
        }
        public void clear() {
            Mapita.Clear();
        }


    }
}
