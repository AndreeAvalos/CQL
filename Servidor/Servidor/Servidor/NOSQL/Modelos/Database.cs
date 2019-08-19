using System;
using System.Collections.Generic;
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

        string link;
        bool exportada = false;

        public Database()
        {
            this.exportada = false;
            this.tablas = new List<Tabla>();
            this.objetos = new List<Objeto>();
            this.procedures = new List<Procedure>();
        }

        public string ArmarHMTL() {

            string salida = "";
            salida += "<h2>TABLAS</h2>\n";
            foreach (Tabla item in tablas) {

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

        public string Name { get => name; set => name = value; }
        public List<Tabla> Tablas { get => tablas; set => tablas = value; }
        public List<Objeto> Objetos { get => objetos; set => objetos = value; }
        public List<Procedure> Procedures { get => procedures; set => procedures = value; }

        public bool Exportada { get => exportada; set => exportada = value; }
        public string Link { get => link; set => link = value; }
    }
}
