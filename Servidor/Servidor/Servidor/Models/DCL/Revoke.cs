using Servidor.NOSQL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models.DCL
{
    public class Revoke : Instruccion
    {

        int line, column;
        List<string> salida;
        string user, id_db;

        public Revoke(int line, int column, string user, string id_db)
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
                int index;
                foreach (Usuario item in Program.sistema.Usuarios)
                {
                    if (item.Name.ToLower().Equals(user))
                    {
                        if (item.Revoke(id_db))
                        {
                            salida.Add(Program.buildMessage("Permiso revocado existasamente."));
                        }
                        else
                        {
                            salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "No existe el permiso en la lista de permisos del usuario " + user + "."));
                        }
                        return null;
                    }
                }
                return null;
            }
            else salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "El usuario " + user + " no existe en el sistema."));
            return null;

        }

        public int getColumn()
        {
            return this.line;
        }

        public int getLine()
        {
            return this.column;
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
