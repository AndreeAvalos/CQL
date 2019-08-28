using Newtonsoft.Json;
using Servidor.NOSQL.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
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
        public bool existUser(string user_name) {
            foreach (Usuario item in usuarios)
            {
                if (item.Name.ToLower().Equals(user_name.ToLower())) return true;
            }
            return false;
        }

        public void deleteDBFROMUSER(string db) {

            foreach (Usuario item in usuarios)
            {
                item.deletePermiso(db);
            }
        }

        public string execComit(int num_tabs)
        {
            string tabulaciones = Program.getTabulaciones(num_tabs);
            string salida = tabulaciones + "\"DATABASES\"= [\n";
            num_tabs++;
            for (int i = 0; i < databases.Count; i++)
            {
                if (i == databases.Count - 1)
                {
                    salida += Program.getTabulaciones(num_tabs) + "<\n";
                    salida += databases.ElementAt(i).execCommit(num_tabs);
                    salida += Program.getTabulaciones(num_tabs) + ">\n";
                }
                else
                {
                    salida += Program.getTabulaciones(num_tabs) + "<\n";
                    salida += databases.ElementAt(i).execCommit(num_tabs);
                    salida += Program.getTabulaciones(num_tabs) + ">,\n";
                }
            }
            salida += tabulaciones + "],\n";
            salida += tabulaciones + "\"USERS\"= [\n";
            for (int i = 0; i < usuarios.Count; i++)
            {
                if (i == usuarios.Count - 1)
                {
                    salida += Program.getTabulaciones(num_tabs) + "<\n";
                    salida += usuarios.ElementAt(i).execCommit(num_tabs);
                    salida += Program.getTabulaciones(num_tabs) + ">\n";
                }
                else
                {
                    salida += Program.getTabulaciones(num_tabs) + "<\n";
                    salida += usuarios.ElementAt(i).execCommit(num_tabs);
                    salida += Program.getTabulaciones(num_tabs) + ">,\n";
                }
            }
            salida += tabulaciones + "]";
            return salida;
        }

        internal bool tienePermiso(string user_actual, string id_db)
        {
            foreach (Usuario item in usuarios)
            {
                if (item.Name.ToLower().Equals(user_actual.ToLower())) {
                    foreach (Permiso item2 in item.Permisos)
                    {
                        if (item2.Name.ToLower().Equals(id_db.ToLower())) return true;
                    }
                    return false;
                }
            }
            return false;
        }

        public bool existPermission(string user_name, string database) {
            foreach (Usuario item in usuarios)
            {
                if (item.Name.ToLower().Equals(user_name.ToLower())) {
                    foreach (Permiso permiso in item.Permisos)
                    {
                        if (permiso.Name.ToLower().Equals(database.ToLower())) return true;
                    }
                    return false;
                }
            }
            return false;
        }

        public bool setPermission(string user_name, Permiso new_permiso)
        {
            foreach (Usuario item in usuarios)
            {
                if (item.Name.ToLower().Equals(user_name.ToLower()))
                {
                    item.Permisos.Add(new_permiso);
                    return true;
                }
            }
            return false;
        }

        internal bool existAtributos(string object_name, string atributo)
        {
            foreach (Database item in databases)
            {
                if (item.En_uso)
                {
                    return item.existAtributo(object_name.ToLower(), atributo);
                }
            }
            return false;
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

        internal bool addAtributo(string object_name, List<Atributo> atributos)
        {
            foreach (Database item in databases)
            {
                if (item.En_uso)
                {
                    return item.addAtributo(object_name.ToLower(), atributos);
                }
            }
            return false;
        }

        internal void deleteAtributo(string id, string name)
        {
            foreach (Database item in databases)
            {
                if (item.En_uso)
                {
                    item.deleteAtributo(id.ToLower(), name.ToLower());
                }
            }
   
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
        public bool dropTable(string name)
        {

            foreach (Database item in databases)
            {
                if (item.En_uso)
                {
                    return item.dropTable(name.ToLower());
                }
            }
            return false;
        }
        public bool truncateTable(string name)
        {
            foreach (Database item in databases)
            {
                if (item.En_uso)
                {
                    return item.truncateTable(name.ToLower());
                }
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

        public bool addColumn(string table_name, Columna column)
        {

            foreach (Database item in databases)
            {
                if (item.En_uso)
                {
                    return item.addColumn(table_name.ToLower(), column);
                }
            }
            return false;

        }

        public bool existColumn(string table_name, string column_name)
        {
            foreach (Database item in databases)
            {
                if (item.En_uso)
                {
                    return item.existeColumn(table_name.ToLower(), column_name);
                }
            }
            return false;
        }

        public bool isPk(string table_name, string column_name)
        {
            foreach (Database item in databases)
            {
                if (item.En_uso)
                {
                    return item.isPK(table_name.ToLower(), column_name);
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


        internal void deleteObjeto(string name)
        {
            foreach (Database item in databases)
            {
                if (item.En_uso)
                {
                    item.deleteObjeto(name);
                }
            }
        }


        public bool addObjeto(Objeto new_objeto)
        {

            foreach (Database item in databases)
            {
                if (item.En_uso)
                {
                    item.Objetos.Add(new_objeto);
                    return true;
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

        public bool getPermission(string name, string database) {
            foreach  (Usuario item in usuarios)
            {
                if (item.Name.ToLower().Equals(name.ToLower())) {
                    foreach (Permiso item2 in item.Permisos)
                    {
                        if (item2.Name.ToLower().Equals(database.ToLower())) return true;
                    }
                    return false;
                }
            }
            return false;
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
