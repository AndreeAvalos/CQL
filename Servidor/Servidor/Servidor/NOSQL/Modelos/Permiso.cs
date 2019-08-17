using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.NOSQL.Modelos
{
    public class Permiso
    {
        string name;

        public Permiso(string name)
        {
            this.name = name;
        }

        public string Name { get => name; set => name = value; }
    }
}
