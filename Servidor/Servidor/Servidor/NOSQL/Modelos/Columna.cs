using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.NOSQL.Modelos
{
    public class Columna
    {
        string name;
        string type;
        bool pk;
        bool modify = false;

        public string Name { get => name; set => name = value; }
        public string Type { get => type; set => type = value; }
        public bool Pk { get => pk; set => pk = value; }
        public bool Modify { get => modify; set => modify = value; }

        public bool isCounter() {
            if (type.ToLower().Equals("counter")) return true;
            return false;
        }
        public bool isPK() {
            if (pk) return true;
            return false;
        }

        public string ArmarRespuesta() {
            return "<th>" + name + "</th>";
        }
        public string ArmarHTML() {
            return "<td>" + name + "</td>" + "<td>" + type + "</td>" + "<td>" + pk + "</td>";
        }

        public string CrearEstructura() {
            string salida = "";
            salida += "\t\t\t\t\t[+COLUMN]\n";
            salida += "\t\t\t\t\t\t[+NAME]\n";
            salida += "\t\t\t\t\t\t\t" + Name + "\n";
            salida += "\t\t\t\t\t\t[-NAME]\n";
            salida += "\t\t\t\t\t[-COLUMN]\n";
            return salida;
        }
    }
}
