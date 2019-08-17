using Servidor.NOSQL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.NOSQL.Estructuras
{
    public class Manejo
    {
        List<Usuario> usuarios;
        List<Database> databases;

        public Manejo()
        {
            this.Usuarios = new List<Usuario>();
            this.Databases = new List<Database>();
        }

        public List<Usuario> Usuarios { get => usuarios; set => usuarios = value; }
        public List<Database> Databases { get => databases; set => databases = value; }
    }
}
