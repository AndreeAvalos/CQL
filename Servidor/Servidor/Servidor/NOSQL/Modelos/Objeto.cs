using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.NOSQL.Modelos
{
    public class Objeto
    {
        string name;
        List<Atributo> atributos = new List<Atributo>();

        public string Name { get => name; set => name = value; }
        public List<Atributo> Atributos { get => atributos; set => atributos = value; }



        public string ArmarRespuesta() {
            string salida = "<table> \n";
            salida += " <tr>\n  ";
            salida += "     <th>NAME</th><th>TYPE</th>\n";
            salida += " </tr>\n";

            foreach (Atributo item in atributos)
            {
                salida += " <tr>\n      ";
                salida += item.ArmarRespuesta() + "\n";
                salida += " </tr>\n";

            }
            salida += "</table> \n";
            return salida;
        }

    }
}
