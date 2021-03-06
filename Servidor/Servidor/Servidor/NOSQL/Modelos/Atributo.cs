﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.NOSQL.Modelos
{
    public class Atributo
    {
        string name;
        string type;
        string attr1, attr2;
        bool collection = false;
        public string Name { get => name; set => name = value; }
        public string Type { get => type; set => type = value; }
        public string Attr1 { get => attr1; set => attr1 = value; }
        public string Attr2 { get => attr2; set => attr2 = value; }
        public bool Collection { get => collection; set => collection = value; }

        public string execCommit(int num_tabs) {
            string tabs = Program.getTabulaciones(num_tabs);
            string salida = tabs;
            salida+="\"NAME\" = \""+ name + "\",\n";
            if(type.ToLower().Equals("set")|| type.ToLower().Equals("list"))
                salida += tabs + "\"TYPE\" = \"" + type + "<"+attr1+">\"\n";
            else if(type.ToLower().Equals("map"))
                salida += tabs + "\"TYPE\" = \"" + type + "<" + attr1+","+attr2 + ">\"\n";
            else
                salida +=tabs+"\"TYPE\" = \"" + type + "\"\n";
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
