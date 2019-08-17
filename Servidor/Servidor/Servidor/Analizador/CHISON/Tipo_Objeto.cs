using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Analizador.CHISON
{
    public class Tipo_Objeto
    {
        string name;
        object valor;

        public Tipo_Objeto(string name, object valor)
        {
            this.name = name;
            this.valor = valor;
        }

        public string Name { get => name; set => name = value; }
        public object Valor { get => valor; set => valor = value; }
    }
}
