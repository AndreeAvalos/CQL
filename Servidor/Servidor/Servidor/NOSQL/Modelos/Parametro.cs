using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.NOSQL.Modelos
{
    public class Parametro
    {
        string name;
        string type;
        string out__;
        bool out_ = false;

        public string Name { get => name; set => name = value; }
        public string Type { get => type; set => type = value; }
        public bool Out_ { get => out_; set => out_ = value; }
        public string Out__ { get => out__; set => out__ = value; }
        public string execCommit(int num_tabs)
        {
            string tabs = Program.getTabulaciones(num_tabs);
            string salida = tabs;
            salida += "\"NAME\" = \"" + name + "\",\n";
            salida += tabs + "\"TYPE\" = \"" + type + "\",\n";
            salida += tabs + "\"AS\" = " + out__ + "\n";
            return salida;
        }
        public string ArmarRespuesta() {
            return "<td>"+name+"</td>"+ "<td>" + type + "</td>"+ "<td>" + out__ + "</td>";
        }
    }
}
