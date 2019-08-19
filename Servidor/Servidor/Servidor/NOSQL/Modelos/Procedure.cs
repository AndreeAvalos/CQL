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

        public string Name { get => name; set => name = value; }
        public string Instr { get => instr; set => instr = value; }
        public List<Parametro> Parametros { get => parametros; set => parametros = value; }
    }
}
