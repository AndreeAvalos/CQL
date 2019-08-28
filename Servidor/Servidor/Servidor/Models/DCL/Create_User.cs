using Servidor.NOSQL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models.DCL
{
    public class Create_User : Instruccion
    {
        int line, column;
        List<string> salida = new List<string>();
        string user, password;

        public Create_User(int line, int column, string user, string password)
        {
            this.line = line;
            this.column = column;
            this.user = user;
            this.password = password;
        }

        public object Ejecutar(TablaDeSimbolos ts)
        {
            if (Program.sistema.existUser(user))
            {
                salida.Add(Program.buildError(getLine(), getColumn(), "Semantico",  user + " UserAlreadyExists."));
            }
            else
            {
                Usuario new_user = new Usuario { Name = user, Password = password };
                Program.sistema.Usuarios.Add(new_user);
                salida.Add(Program.buildMessage("Usuario creado exitosamente."));
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
    }
}
