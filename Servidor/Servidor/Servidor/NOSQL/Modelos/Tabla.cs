using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.NOSQL.Modelos
{
    public class Tabla
    {
        string name = "";
        List<Columna> columnas;
        List<Fila> filas;

        string link;
        bool exportada = false;

        public string Name { get => name; set => name = value; }
        public List<Columna> Columnas { get => columnas; set => columnas = value; }
        public List<Fila> Filas { get => filas; set => filas = value; }
        public bool Exportada { get => exportada; set => exportada = value; }
        public string Link { get => link; set => link = value; }

        public Tabla()
        {
            this.columnas = new List<Columna>();
            this.filas = new List<Fila>();
        }

        public bool existeColumn(string val)
        {
            foreach (Columna item in columnas)
            {
                if (item.Name.ToLower().Equals(val)) return true;
            }
            return false;
        }

        public string execColumn(int num_tabs) {
            string tabs = Program.getTabulaciones(num_tabs);
            num_tabs++;
            string salida = "";
            string tabs2 = Program.getTabulaciones(num_tabs);
            for (int i = 0; i < columnas.Count; i++)
            {
                if (i == columnas.Count - 1)
                {
                    salida += tabs + "<\n";
                    salida += tabs2 + "\"NAME\"= \"" + columnas.ElementAt(i).Name + "\",\n";
                    salida += tabs2 + "\"TYPE\"= \"" + columnas.ElementAt(i).Type + "\",\n";
                    salida += tabs2 + "\"PK\"= " + columnas.ElementAt(i).Pk.ToString().ToUpper() + "\n";
                    salida += tabs + ">\n";
                }
                else {
                    salida += tabs + "<\n";
                    salida += tabs2 + "\"NAME\"= \"" + columnas.ElementAt(i).Name + "\",\n";
                    salida += tabs2 + "\"TYPE\"= \"" + columnas.ElementAt(i).Type + "\",\n";
                    salida += tabs2 + "\"PK\"= " + columnas.ElementAt(i).Pk.ToString().ToUpper() + "\n";
                    salida += tabs + ">,\n";
                }
            }
            return salida;
        }

        public string execCommit(int num_tabs) {
            string tabs = Program.getTabulaciones(num_tabs);
            
            if (exportada)
            {
                string salida = "";
                for (int i = 0; i < filas.Count; i++)
                {
                    if (filas.Count - 1 == i)
                    {
                       salida+= filas.ElementAt(i).writeImportFile()+"\n";
                    }
                    else {
                      salida+=  filas.ElementAt(i).writeImportFile()+",\n";
                    }
                }
                File.WriteAllText("./NOSQL/Generados/"+ link, salida);
                return tabs+ "${ " + link + " }$\n";
            }
            else {
                string salida = "";
                for (int i = 0; i < filas.Count; i++)
                {

                    if (filas.Count - 1 == i)
                    {
                        salida += filas.ElementAt(i).writeImportFile() + "\n";
                    }
                    else
                    {
                        salida += filas.ElementAt(i).writeImportFile() + ",\n";
                    }
                }


                return salida ;
            }

        }

        internal bool isPk(string column_name)
        {
            foreach (Columna item in columnas)
            {
                if (item.Name.ToLower().Equals(column_name)) return item.isPK();
            }
            return false;
        }

        public bool dropColumn(string name)
        {
            int index = -1;
            for (int i = 0; i < columnas.Count; i++)
            {
                if (columnas.ElementAt(i).Name.ToLower().Equals(name.ToLower()))
                {
                    index = i;
                    break;
                }
            }
            if (index != -1) { columnas.RemoveAt(index); return true; }
            else return false;
        }

        public string ArmarRespuesta()
        {
            string salida = "<table> \n";
            salida += " <tr>\n  ";
            foreach (Columna item in Columnas)
            {
                salida += item.ArmarRespuesta().ToString();
            }
            salida += "\n </tr>\n";

            foreach (Fila item in Filas)
            {
                salida += " <tr>\n      ";
                salida += item.ArmarRespuesta(columnas) + "\n";
                salida += " </tr>\n";
            }

            salida += "</table> \n";
            return salida;
        }


        public string ArmarHTML()
        {
            string salida = "<table> \n";
            salida += "\t<tr>\n\t\t<th>NAME</th><th>TYPE</th><th>PK</th>\n\t</tr>\n";
            foreach (Columna item in Columnas)
            {

                salida += "\t\t<tr>" + item.ArmarHTML() + "</tr>\n";
            }
            salida += "</table> \n";
            return salida;
        }


        public string CrearEstructura()
        {
            string salida = "";
            salida += "\t\t\t[+TABLE]\n";
            salida += "\t\t\t\t[+NAME]\n";
            salida += "\t\t\t\t\t" + Name + "\n";
            salida += "\t\t\t\t[-NAME]\n";
            salida += "\t\t\t\t[+COLUMNS]\n";
            foreach (Columna item in columnas)
            {
                salida += item.CrearEstructura();
            }
            salida += "\t\t\t\t[-COLUMNS]\n";
            salida += "\t\t\t[-TABLE]\n";
            return salida;
        }
    }
}
