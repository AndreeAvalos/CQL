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

        string padre;

        string link = "";
        bool export = false;

        int type;

        public Tipo_Objeto(string name, object valor)
        {
            this.name = name;
            this.valor = valor;
        }

        public string Name { get => name; set => name = value; }
        public object Valor { get => valor; set => valor = value; }
        public string Link { get => link; set => link = value; }
        public bool Export { get => export; set => export = value; }
        public string Padre { get => padre; set => padre = value; }
        public int Type { get => type; set => type = value; }
    }
}
