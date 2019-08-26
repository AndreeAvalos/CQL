using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.NOSQL.Modelos
{
    public class Database
    {
        string name;
        List<Tabla> tablas;
        List<Objeto> objetos;
        List<Procedure> procedures;
        bool en_uso;
        string link;
        bool exportada = false;

        public Database()
        {
            this.exportada = false;
            this.tablas = new List<Tabla>();
            this.objetos = new List<Objeto>();
            this.procedures = new List<Procedure>();
            this.en_uso = false;
        }

        public bool existTable(string name)
        {
            foreach (Tabla item in Tablas)
            {
                if (item.Name.ToLower().Equals(name.ToLower())) return true;
            }
            return false;
        }

        public bool existeObjeto(string name)
        {
            foreach (Objeto item in objetos)
            {
                if (item.Name.ToLower().Equals(name.ToLower())) return true;
            }
            return false;
        }
        public bool addColumn(string table_name, Columna column)
        {
            foreach (Tabla item in tablas)
            {
                if (item.Name.ToLower().Equals(table_name)) { item.Columnas.Add(column); return true; }
            }
            return false;
        }

        public bool existeColumn(string table_name, string column_name)
        {

            foreach (Tabla item in tablas)
            {
                if (item.Name.ToLower().Equals(table_name)) { return item.existeColumn(column_name); }
            }
            return false;

        }
        public bool dropColumn(string table_name, string column_name)
        {
            foreach (Tabla item in tablas)
            {
                if (item.Name.ToLower().Equals(table_name)) { return item.dropColumn(column_name); }
            }
            return false;
        }
        internal bool isPK(string table_name, string column_name)
        {
            foreach (Tabla item in tablas)
            {
                if (item.Name.ToLower().Equals(table_name)) { return item.isPk(column_name); }
            }
            return false;
        }

        public bool dropTable(string nombre)
        {
            int index = -1;
            for (int i = 0; i < tablas.Count; i++)
            {
                if (tablas.ElementAt(i).Name.ToLower().Equals(nombre.ToLower())) index = i;
            }

            if (index != -1)
            {
                Tablas.RemoveAt(index);
                return true;
            }
            return false;
        }

        public string execCommit(int num_tabs)
        {
            num_tabs++;
            string tabs = Program.getTabulaciones(num_tabs);
            num_tabs++;
            string salida = tabs + "\"NAME\"= \"" + name + "\",\n";
            salida += tabs + "\"DATA\"= [\n";
            if (exportada)
            {
                salida += Program.getTabulaciones(num_tabs) + "${ " + link + " }$\n";
                writeImportFile();
            }
            else
            {
                for (int i = 0; i < tablas.Count; i++)
                {
                    if (i == tablas.Count - 1)
                    {
                        salida += Program.getTabulaciones(num_tabs) + "<\n";
                        salida += Program.getTabulaciones(num_tabs+1)+"\"CQL-TYPE\" = \"TABLE\",\n";
                        salida += Program.getTabulaciones(num_tabs + 1) + "\"NAME\" = \"" + tablas.ElementAt(i).Name + "\"\n";
                        salida += Program.getTabulaciones(num_tabs + 1) + "\"COLUMNS\" = [\n";
                        salida += Program.getTabulaciones(num_tabs + 1) + "],\n";
                        salida += Program.getTabulaciones(num_tabs + 1) + "\"DATA\" = [\n";
                        salida += Program.getTabulaciones(num_tabs + 1) + "]\n";
                        salida += Program.getTabulaciones(num_tabs) + ">\n";
                    }
                    else
                    {
                        salida += Program.getTabulaciones(num_tabs) + "<\n";
                        salida += Program.getTabulaciones(num_tabs + 1) + "\"CQL-TYPE\" = \"TABLE\",\n";
                        salida += Program.getTabulaciones(num_tabs + 1) + "\"NAME\" = \"" + tablas.ElementAt(i).Name + "\"\n";
                        salida += Program.getTabulaciones(num_tabs + 1) + "\"COLUMNS\" = [\n";
                        salida += Program.getTabulaciones(num_tabs + 1) + "],\n";
                        salida += Program.getTabulaciones(num_tabs + 1) + "\"DATA\" = [\n";
                        salida += Program.getTabulaciones(num_tabs + 1) + "]\n";
                        salida += Program.getTabulaciones(num_tabs) + ">,\n";
                    }
                }
            }
            salida += tabs + "]\n";
            return salida;
        }
        private void writeImportFile()
        {
            string path = "P"+link;
            string text = "<\n";
            text += ">";
            File.WriteAllText(path, text);
        }

        public string ArmarHMTL()
        {

            string salida = "";
            salida += "<h2>TABLAS</h2>\n";
            foreach (Tabla item in tablas)
            {

                salida += item.ArmarRespuesta() + "\n";
            }
            salida += "<h2>OBJETOS</h2>\n";
            foreach (Objeto item in objetos)
            {

                salida += item.ArmarRespuesta() + "\n";
            }
            salida += "<h2>PROCEDURES</h2>\n";
            foreach (Procedure item in procedures)
            {

                salida += item.ArmarRespuesta() + "\n";
            }
            return salida;
        }

        internal bool truncateTable(string name)
        {
            foreach (Tabla item in Tablas)
            {
                if (item.Name.ToLower().Equals(name)) { item.Filas.Clear(); return true; }
            }
            return false;
        }

        public string CrearEstructura()
        {
            string salida = "";
            salida += "\t[+DATABASE]\n";
            salida += "\t\t[+NAME]\n";
            salida += "\t\t\t" + Name + "\n";
            salida += "\t\t[-NAME]\n";
            salida += "\t\t[+TABLES]\n";
            foreach (Tabla item in Tablas)
            {
                salida += item.CrearEstructura();
            }
            salida += "\t\t[-TABLES]\n";
            salida += "\t\t[+TYPES]\n";
            foreach (Objeto item in Objetos)
            {
                salida += item.CrearEstructura();
            }
            salida += "\t\t[-TYPES]\n";
            salida += "\t\t[+PROCEDURES]\n";
            foreach (Procedure item in Procedures)
            {
                salida += item.CrearEstructura();
            }
            salida += "\t\t[-PROCEDURES]\n";


            salida += "\t[-DATABASE]\n";
            return salida;
        }

        public string Name { get => name; set => name = value; }
        public List<Tabla> Tablas { get => tablas; set => tablas = value; }
        public List<Objeto> Objetos { get => objetos; set => objetos = value; }
        public List<Procedure> Procedures { get => procedures; set => procedures = value; }



        public bool Exportada { get => exportada; set => exportada = value; }
        public string Link { get => link; set => link = value; }
        public bool En_uso { get => en_uso; set => en_uso = value; }
    }
}
