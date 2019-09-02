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
        string user_actual;

        public Grant(int line, int column, string user, string id_db, string user_actual)
        {
            this.line = line;
            this.column = column;
            this.salida = new List<string>();
            this.user = user;
            this.id_db = id_db;
            this.user_actual = user_actual;
        }
        public void clearSalida()
        {
            this.salida.Clear();
        }

        public object Ejecutar(TablaDeSimbolos ts)
        {
            if (!Program.sistema.tienePermiso(user_actual, id_db))
            {
                salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "El usuario " + user_actual + " no posee permisos en " + id_db + "."));
            }
            else
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
                        else salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", id_db + " BDDontExists."));
                    }
                    else salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "El usuario " + user + " ya tiene permiso sobre " + id_db + "."));
                }
                else salida.Add(Program.buildError(getLine(), getColumn(), "Semantico",  user + " UserDontExists."));
            }
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

        public Tipo getType()
        {
            return Tipo.DCL;
        }
    }
}
