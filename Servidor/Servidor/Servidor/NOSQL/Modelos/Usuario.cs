using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.NOSQL.Modelos
{
    public class Usuario
    {
        string name;
        string password;
        List<Permiso> permisos = new List<Permiso>();

        public string execCommit(int num_tabs)
        {
            num_tabs++;
            string tabs = Program.getTabulaciones(num_tabs);
            num_tabs++;
            string salida = tabs + "\"NAME\"= \"" + name + "\",\n";
            salida += tabs + "\"PASSWORD\"= \"" + password + "\",\n";
            salida += tabs + "\"PERMISSIONS\"= [\n";
            tabs = Program.getTabulaciones(num_tabs);
            for (int i = 0; i < permisos.Count; i++)
            {
                if (i == permisos.Count - 1)
                {
                    salida += tabs + "<\n";
                    salida += permisos.ElementAt(i).execCommit(num_tabs + 1);
                    salida += tabs + ">\n";
                }
                else
                {
                    salida += tabs + "<\n";
                    salida += permisos.ElementAt(i).execCommit(num_tabs + 1);
                    salida += tabs + ">,\n";
                }
            }
            tabs = Program.getTabulaciones(num_tabs-1);
            salida += tabs + "]\n";
            return salida;
        }

        public string Name { get => name; set => name = value; }
        public string Password { get => password; set => password = value; }
        public List<Permiso> Permisos { get => permisos; set => permisos = value; }
    }
}
