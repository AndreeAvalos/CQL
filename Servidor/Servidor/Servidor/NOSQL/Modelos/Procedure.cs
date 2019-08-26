using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.NOSQL.Modelos
{
    public class Procedure
    {
        string name;
        string instr;
        List<Parametro> parametros = new List<Parametro>();


        public string ArmarRespuesta()
        {
            string salida = "<table> \n";
            salida += " <tr>\n  ";
            salida += "     <th>NAME</th><th>TYPE</th><th>AS</th>\n";
            salida += " </tr>\n";

            foreach (Parametro item in parametros)
            {
                salida += " <tr>\n      ";
                salida += item.ArmarRespuesta() + "\n";
                salida += " </tr>\n";

            }
            salida += "</table> \n";
            return salida;
        }
        public string CrearEstructura()
        {
            string salida = "";
            salida += "\t\t\t[+PROCEDURE]\n";
            salida += "\t\t\t\t[+NAME]\n";
            salida += "\t\t\t\t\t" + Name + "\n";
            salida += "\t\t\t\t[-NAME]\n";
            salida += "\t\t\t[-PROCEDURE]\n";

            return salida;

        }
        public string Name { get => name; set => name = value; }
        public string Instr { get => instr; set => instr = value; }
        public List<Parametro> Parametros { get => parametros; set => parametros = value; }

        internal string execCommit(int num_tabs)
        {
            string tabs = Program.getTabulaciones(num_tabs);
            string salida = "";
            for (int i = 0; i < parametros.Count; i++)
            {
                if (i == parametros.Count - 1)
                {
                    salida += tabs + "<\n";
                    salida += parametros.ElementAt(i).execCommit(num_tabs + 1);
                    salida += tabs + ">\n";
                }
                else
                {
                    salida += tabs + "<\n";
                    salida += parametros.ElementAt(i).execCommit(num_tabs + 1);
                    salida += tabs + ">,\n";

                }

            }
            return salida;
        }
    }
}
