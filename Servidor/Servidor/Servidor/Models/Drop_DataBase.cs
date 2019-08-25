using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public class Drop_DataBase : Instruccion
    {
        private string id;
        public List<string> salida = new List<string>();
        int linea, columna;
        public Drop_DataBase(string id, int line, int column)
        {
            this.id = id;
            this.linea = line;
            this.columna = column;
        }

        public object Recolectar(TablaDeSimbolos ts) { return null; }
        public object Ejecutar(TablaDeSimbolos ts)
        {
            if (Program.sistema.existDataBase(id.ToLower()))
            {
                Program.sistema.deleteDataBase(id);
                salida.Add(Program.buildMessage("La base de datos " + id + " fue eliminada con exito."));

            }
            else
            {
                //informar que no existe database

                salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "La base de datos que desea eliminar no existe."));

            }
            return null;
        }
        public List<string> getSalida()
        {

            return salida;
        }
        public int getLine()
        {
            return linea;
        }
        public int getColumn()
        {
            return columna;
        }
    }
}
