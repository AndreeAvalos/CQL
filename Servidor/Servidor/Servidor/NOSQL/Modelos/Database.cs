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

        string anexo;
        bool exportada = false;

        public Database()
        {
            this.exportada = false;
            this.tablas = new List<Tabla>();
            this.objetos = new List<Objeto>();
            this.procedures = new List<Procedure>();
        }

        public string ArmarHMTL(int tabla) {

            string salida = "";
            foreach (Tabla item in tablas) {
                salida += item.ArmarHTML() + "\n";
            }
            return salida;
        }

        public string Name { get => name; set => name = value; }
        public List<Tabla> Tablas { get => tablas; set => tablas = value; }
        public List<Objeto> Objetos { get => objetos; set => objetos = value; }
        public List<Procedure> Procedures { get => procedures; set => procedures = value; }
        public string Anexo { get => anexo; set => anexo = value; }
        public bool Exportada { get => exportada; set => exportada = value; }
    }
}
