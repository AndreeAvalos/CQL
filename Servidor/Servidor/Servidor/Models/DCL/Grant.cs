using Servidor.NOSQL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models.DCL
{
    public class Grant : Instruccion
    {

        int line, column;
        List<string> salida;
        string user, id_db;

        public Grant(int line, int column, string user, string id_db)
        {
            this.line = line;
            this.column = column;
            this.salida = new List<string>();
            this.user = user;
            this.id_db = id_db;
        }

        public object Ejecutar(TablaDeSimbolos ts)
        {
            if (Program.sistema.existUser(user))
            {
                if (!Program.sistema.existPermission(user, id_db))
                {
                    if (Program.sistema.existDataBase(id_db))
                    {
                        string bd_correct = "";

                        foreach (Database item in Program.sistema.Databases)
                        {
                            if (item.Name.ToLower().Equals(id_db)) bd_correct = item.Name;
                        }
                        Permiso new_permiso = new Permiso(bd_correct);
                        if (Program.sistema.setPermission(user, new_permiso)) salida.Add(Program.buildMessage("Permisos concedidos a " + user + " para base de datos " + id_db + "."));
                        else salida.Add(Program.buildMessage("Error interno del sistema."));
                    }
                    else salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "La base de datos " + id_db + " no existe en el sistema."));
                }
                else salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "El usuario " + user + " ya tiene permiso sobre " + id_db + "."));
            }
            else salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "El usuario " + user + " no existe en el sistema."));
            return null;
        }

        public int getColumn()
        {
            return this.column;
        }

        public int getLine()
        {
            return this.line;
        }

        public List<string> getSalida()
        {
            return this.salida;
        }

        public object Recolectar(TablaDeSimbolos ts)
        {
            throw new NotImplementedException();
        }
    }
}
