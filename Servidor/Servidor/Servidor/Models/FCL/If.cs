using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models.FCL
{
    public class If : Instruccion
    {
        int line, column;
        List<string> salida = new List<string>();


        public object Ejecutar(TablaDeSimbolos ts)
        {
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
            return null;
        }
    }
}
