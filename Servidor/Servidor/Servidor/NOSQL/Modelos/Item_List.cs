using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.NOSQL.Modelos
{
    public class Item_List
    {
        int type;
        object valor;

        public Item_List(int type, object valor)
        {
            this.Type = type;
            this.Valor = valor;
        }

        public int Type { get => type; set => type = value; }
        public object Valor { get => valor; set => valor = value; }
    }
}
