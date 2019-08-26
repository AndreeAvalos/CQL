using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models.TCL
{
    public class Rollback : Instruccion
    {
        List<string> salida = new List<string>();
        public object Ejecutar(TablaDeSimbolos ts)
        {
            Program.execRollbak();
            salida.Add(Program.buildMessage("Se ha restaurado la base de datos."));
            return null;
        }

        public int getColumn()
        {
            throw new NotImplementedException();
        }

        public int getLine()
        {
            throw new NotImplementedException();
        }

        public List<string> getSalida()
        {
            return salida;
        }

        public object Recolectar(TablaDeSimbolos ts)
        {
            throw new NotImplementedException();
        }
    }
}
