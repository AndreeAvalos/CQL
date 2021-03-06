﻿using System;
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
            tabs = Program.getTabulaciones(num_tabs - 1);
            salida += tabs + "]\n";
            return salida;
        }

        internal void deletePermiso(string db)
        {
            int index = -1;
            for (int i = 0; i < permisos.Count; i++)
            {
                if (permisos.ElementAt(i).Name.ToLower().Equals(db.ToLower())) index = i;
            }
            if (index != -1) permisos.RemoveAt(index);
            return;
        }

        public bool Revoke(string id)
        {
            int index = -1;
            for (int i = 0; i < Permisos.Count; i++)
            {
                if (Permisos.ElementAt(i).Name.ToLower().Equals(id.ToLower())) index = i;
            }
            if (index != -1) { Permisos.RemoveAt(index); return true; }
            else return false;
        }
        public string Name { get => name; set => name = value; }
        public string Password { get => password; set => password = value; }
        public List<Permiso> Permisos { get => permisos; set => permisos = value; }
    }
}
