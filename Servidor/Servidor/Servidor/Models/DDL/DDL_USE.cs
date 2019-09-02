using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public class DDL_USE : Instruccion
    {
        private string id;
        private string user;
        public List<string> salida = new List<string>();
        int linea, columna;
        public DDL_USE(string id, int line, int column, string user)
        {
            this.id = id;
            this.linea = line;
            this.columna = column;
            this.user = user;
        }
        public Tipo getType()
        {
            return Tipo.DDL;
        }

        public object Recolectar(TablaDeSimbolos ts)
        {
            return null;
        }
        public object Ejecutar(TablaDeSimbolos ts)
        {
            if (Program.sistema.getPermission(user, id))
            {
                if (Program.sistema.existDataBase(id.ToLower()))
                {
                    Program.sistema.asignUse(id);
                    salida.Add(Program.buildMessage("Base de datos en uso: " + id));
                }
                else
                {
                    //informar que no existe database
                    salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "BDDontExists"));
                }
            }
            else salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "El usuario no posee permisos para usar la base de datos."));
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
