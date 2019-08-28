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
        string attr1, attr2;
        bool pk;
        bool collection = false;

        public string Name { get => name; set => name = value; }
        public string Type { get => type; set => type = value; }
        public bool Pk { get => pk; set => pk = value; }
        public string Attr1 { get => attr1; set => attr1 = value; }
        public string Attr2 { get => attr2; set => attr2 = value; }
        public bool Collection { get => collection; set => collection = value; }

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
