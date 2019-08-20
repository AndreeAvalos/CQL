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

        public bool Buscar_Usuario(string user, string password)
        {
            foreach (Usuario item in usuarios)
            {
                if (item.Name.Equals(user))
                {
                    if (item.Password.Equals(password)) return true;
                    else return false;
                }
            }

            return false;
        }

        public string Crear_Estructura(string usuario)
        {
            string salida = "";
            Usuario usuario_actual = new Usuario();

            foreach (Usuario item in usuarios)
            {
                if (item.Name.Equals(usuario))
                {
                    usuario_actual = item;
                    break;
                }
            }

            if (usuario_actual.Permisos.Count != 0)
            {
                salida = "[+DATABASES]\n";
                foreach (Permiso item in usuario_actual.Permisos)
                {
                    Database database_actual = getDataBase(item.Name);
                    if (database_actual != null)
                    {
                        salida += database_actual.CrearEstructura();
                    }
                }
                salida += "[-DATABASES]";
            }
            else
            {
                salida = "";
            }



            return salida;
        }

        private Database getDataBase(string name)
        {
            foreach (Database item in databases)
            {
                if (item.Name.Equals(name)) return item;
            }
            return null;
        }
    }
}
