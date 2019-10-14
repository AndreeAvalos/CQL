using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public class Create_DataBase : Instruccion
    {
        private bool existe;
        private string id;
        private string user;
        public List<string> salida = new List<string>();
        int linea, columna;
        public Create_DataBase(bool existe, string id,string user, int line, int column)
        {
            this.existe = existe;
            this.id = id;
            this.user = user;
            this.linea = line;
            this.columna = column;
        }
        public List<string> getSalida()
        {

            return salida;
        }
        public void clearSalida()
        {
            this.salida.Clear();
        }

        public object Recolectar(TablaDeSimbolos ts) {
            return null;
        }
        public Object Ejecutar(TablaDeSimbolos ts) {
            if (Program.sistema.existDataBase(id) == true)
            {
                if (existe == true)
                {
                    //no hacer nada
                }
                else
                {
                    //Imprimir error ya que la tabla 
                    salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", id+ " BDAlreadyExists."));
                }
            }
            else {
                Program.sistema.addDataBase(id);
                Program.sistema.addPermissions(user, id);
                Program.sistema.addPermissions("admin", id);
                salida.Add(Program.buildMessage("La base de datos fue creada con exito."));

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
        public Tipo getType()
        {
            return Tipo.DDL;
        }
    }
}
