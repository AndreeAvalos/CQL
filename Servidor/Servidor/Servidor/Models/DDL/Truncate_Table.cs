using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public class Truncate_Table : Instruccion
    {
        string id_tabla;
        public List<string> salida = new List<string>();
        int linea, columna;

        public Truncate_Table(string id_tabla, int line, int column)
        {
            this.id_tabla = id_tabla;
            this.linea = line;
            this.columna = column;
        }
        public List<string> getSalida()
        {

            return salida;
        }
        public Tipo getType()
        {
            return Tipo.DDL;
        }


        public object Recolectar(TablaDeSimbolos ts) { return null; }
        public object Ejecutar(TablaDeSimbolos ts)
        {
            if (Program.sistema.En_uso())
            {
                if (Program.sistema.existTable(id_tabla.ToLower()))
                {
                    bool eliminado = Program.sistema.truncateTable(id_tabla.ToLower());
                    if (eliminado)
                    {
                        //informar que se elimino con exito
                        salida.Add(Program.buildMessage("Tabla " + id_tabla + " truncada con exito."));
                    }
                    else
                    {
                        // informar que es un error interno;
                    }
                }
                else
                {
                    salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "La tabla "+id_tabla+" no existe en la base de datos actual."));

                }
            }
            else
            {
                //no hay ninguna base de datos seleccionada.
                salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "UseBDException."));
            }

            return null;
        }
        public int getLine()
        {
            return linea;
        }
        public int getColumn() {
            return columna;
        }
    }
}
