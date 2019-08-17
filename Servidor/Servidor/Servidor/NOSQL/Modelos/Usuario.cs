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
        List<Permiso> permisos;

        public string Name { get => name; set => name = value; }
        public string Password { get => password; set => password = value; }
        public List<Permiso> Permisos { get => permisos; set => permisos = value; }
    }
}
