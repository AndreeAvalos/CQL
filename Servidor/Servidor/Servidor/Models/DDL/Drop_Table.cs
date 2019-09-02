using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public class Drop_Table : Instruccion
    {
        string id_tabla;
        bool existe;
        int linea, columna;
        public List<string> salida = new List<string>();
        public Drop_Table(string id_tabla, bool existe, int line, int column)
        {
            this.id_tabla = id_tabla;
            this.existe = existe;
            this.linea = line;
            this.columna = column;
        }
        public Tipo getType()
        {
            return Tipo.DDL;
        }

        public List<string> getSalida()
        {

            return salida;
        }

        public void clearSalida()
        {
            this.salida.Clear();
        }
        public object Recolectar(TablaDeSimbolos ts) { return null; }
        public object Ejecutar(TablaDeSimbolos ts)
        {
            if (Program.sistema.En_uso())
            {
                if (!Program.sistema.existTable(id_tabla.ToLower()))
                {
                    if (existe == true)
                    {
                        //no hace nada
                    }
                    else
                    {
                        salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", id_tabla+ " TableDontExists."));
                        // marcar error
                    }
                }
                else
                {
                    bool eliminado = Program.sistema.dropTable(id_tabla.ToLower());
                    if (eliminado)
                    {
                        //mandar mensaje de eliminacion
                        salida.Add(Program.buildMessage("La tabla " + id_tabla + " fue eliminida con exito."));
                    }
                    else
                    {
                        // mandar mensaje que hubo un error interno.
                        salida.Add(Program.buildMessage("ha ocurrido un error interno mientras se intentaba eliminar la tabla."));
                        
                    }

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
        public int getColumn()
        {
            return columna;
        }
    }
}
