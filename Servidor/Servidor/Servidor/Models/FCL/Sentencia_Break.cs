using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models.FCL
{
    public class Sentencia_Break : Instruccion
    {
        int linea, columna;
        List<string> salida = new List<string>();
        public Sentencia_Break(int linea, int columna)
        {
            this.linea = linea;
            this.columna = columna;
        }
        public void clearSalida()
        {
            this.salida.Clear();
        }

        public object Ejecutar(TablaDeSimbolos ts)
        {

            return null;
        }

        public int getColumn()
        {
            return columna;
        }

        public int getLine()
        {
            return linea;
        }

        public List<string> getSalida()
        {
            return salida;
        }

        public Tipo getType()
        {
            return Tipo.BREAK;
        }

        public object Recolectar(TablaDeSimbolos ts)
        {
            return null;
        }
    }
}
