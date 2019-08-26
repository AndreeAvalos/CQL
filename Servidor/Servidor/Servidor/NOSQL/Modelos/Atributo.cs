using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.NOSQL.Modelos
{
    public class Atributo
    {
        string name;
        string type;

        public string Name { get => name; set => name = value; }
        public string Type { get => type; set => type = value; }

        public string execCommit(int num_tabs) {
            string tabs = Program.getTabulaciones(num_tabs);
            string salida = tabs;
            salida+="\"NAME\" = \""+ name + "\",\n";
            salida+=tabs+"\"TYPE\" = \"" + type + "\"\n";
            return salida;
        }

        public string CrearEstructura() {
            string salida = "";
            salida += "\t\t\t\t\t[+ATTRIBUTE]\n";
            salida += "\t\t\t\t\t\t[+NAME]\n";
            salida += "\t\t\t\t\t\t\t" + Name + "\n";
            salida += "\t\t\t\t\t\t[-NAME]\n";
            salida += "\t\t\t\t\t[-ATTRIBUTE]\n";
            return salida;
        }
        public string ArmarRespuesta() {
            return "<td>" + name + "</td>" + "<td>" + type + "</td>";
        }
    }
}
