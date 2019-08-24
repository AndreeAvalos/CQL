using Servidor.NOSQL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public class Create_Table : Instruccion
    {
        private string id;
        List<Columna> columnas;
        List<Fila> filas;
        bool existe;
        bool compuesta;

        public Create_Table(string id, List<Columna> columnas, bool existe, bool compuesta)
        {
            this.id = id;
            this.columnas = columnas;
            this.filas = new List<Fila>();
            this.existe = existe;
            this.compuesta = compuesta;
        }

        public object Recolectar(TablaDeSimbolos ts) { return null; }
        public object Ejecutar(TablaDeSimbolos ts)
        {
            return null;
        }
    }
}
