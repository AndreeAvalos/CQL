using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public class Item_Map
    {
        string clave;
        object valor;

        public Item_Map(string clave, object valor)
        {
            this.clave = clave;
            this.valor = valor;
        }

        public string Clave { get => clave; set => clave = value; }
        public object Valor { get => valor; set => valor = value; }
    }
}
