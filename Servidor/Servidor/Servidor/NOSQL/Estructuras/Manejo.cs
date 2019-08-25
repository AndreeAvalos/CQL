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

        public bool En_uso()
        {

            foreach (Database item in databases)
            {
                if (item.En_uso) return true;
            }
            return false;
        }

        private Database getDataBase(string name)
        {
            foreach (Database item in databases)
            {
                if (item.Name.ToLower().Equals(name.ToLower())) return item;
            }
            return null;
        }
        public bool existDataBase(string name)
        {
            foreach (Database item in databases)
            {
                if (item.Name.ToLower().Equals(name.ToLower())) return true;
            }
            return false;
        }

        public bool existTable(string name)
        {

            foreach (Database item in databases)
            {
                if (item.En_uso)
                {
                    return item.existTable(name);
                }
            }
            return false;
        }

        public bool addColumn(string table_name, Columna column) {

            foreach (Database item in databases)
            {
                if (item.En_uso)
                {
                    return item.addColumn(table_name.ToLower(), column);
                }
            }
            return false;

        }

        public bool existColumn(string table_name, string column_name) {
            foreach (Database item in databases)
            {
                if (item.En_uso)
                {
                    return item.existeColumn(table_name.ToLower(), column_name);
                }
            }
            return false;
        }

        public bool dropColumn(string table_name, string column_name)
        {
            foreach (Database item in databases)
            {
                if (item.En_uso)
                {
                    return item.dropColumn(table_name.ToLower(), column_name);
                }
            }
            return false;
        }

        public bool existeObjeto(string name)
        {
            foreach (Database item in databases)
            {
                if (item.En_uso)
                {
                    return item.existeObjeto(name);
                }
            }
            return false;

        }


        public bool addTable(Tabla new_tabla)
        {

            foreach (Database item in databases)
            {
                if (item.En_uso)
                {
                    item.Tablas.Add(new_tabla);
                    return true;
                }
            }
            return false;

        }
        public void addPermissions(string name, string database)
        {
            foreach (Usuario item in usuarios)
            {
                if (item.Name.ToLower().Equals(name.ToLower()))
                {
                    item.Permisos.Add(new Permiso(database));
                    return;
                }
            }
        }
        public void addDataBase(string name)
        {
            Database db = new Database();
            db.Name = name;
            Databases.Add(db);
        }

        //Metodo para manejar que database se usa, la primer base de datos 
        public void asignUse(string name)
        {
            foreach (Database item in databases)
            {
                if (item.Name.ToLower().Equals(name.ToLower())) item.En_uso = true;
                else item.En_uso = false;
            }
        }
        public void deleteDataBase(string name)
        {
            int index = -1;
            for (int i = 0; i < databases.Count; i++)
            {
                if (databases.ElementAt(i).Name.ToLower().Equals(name.ToLower()))
                {
                    index = i;
                    break;
                }
            }
            if (index != -1) databases.RemoveAt(index);
        }
    }
}
