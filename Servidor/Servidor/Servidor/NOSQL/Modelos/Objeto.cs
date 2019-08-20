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

        public string CrearEstructura() {
            string salida = "";
            salida += "\t\t\t[+TYPE]\n";
            salida += "\t\t\t\t[+NAME]\n";
            salida += "\t\t\t\t\t" + Name + "\n";
            salida += "\t\t\t\t[-NAME]\n";
            salida += "\t\t\t\t[+ATTRIBUTES]\n";
            foreach (Atributo item in atributos)
            {
                salida += item.CrearEstructura();
            }
            salida += "\t\t\t\t[-ATTRIBUTES]\n";
            salida += "\t\t\t[-TYPE]\n";

            return salida;
        }

    }
}
