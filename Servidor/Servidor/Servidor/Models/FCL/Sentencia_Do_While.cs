using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models.FCL
{
    public class Sentencia_Do_While : Instruccion
    {
        int line, column;
        List<string> salida = new List<string>();
        Operacion expresion;
        LinkedList<Instruccion> instruccions;

        public Sentencia_Do_While(Operacion expresion, LinkedList<Instruccion> instruccions, int line, int column)
        {
            this.line = line;
            this.column = column;
            this.expresion = expresion;
            this.instruccions = instruccions;
        }
        public void clearSalida()
        {
            this.salida.Clear();
        }

        public object Ejecutar(TablaDeSimbolos ts)
        {
            int limite = 0;
            do
            {
                TablaDeSimbolos tabla_local = new TablaDeSimbolos();
                tabla_local.addPadre(ts);
                
                foreach (Instruccion item in instruccions)
                {
                    if (item.getType() == Tipo.BREAK) return null;
                    if (item.getType() == Tipo.USER_TYPES || item.getType() == Tipo.FUNCION || item.getType() == Tipo.METODO || item.getType() == Tipo.PROCEDURE)
                    {
                        salida.Add(Program.buildError(item.getLine(), item.getColumn(), "Semantico", "No puede venir instruccion del tipo: " + item.getType() + " en un ambito local."));
                    }
                    else
                    {
                        item.Ejecutar(tabla_local);
                        salida.AddRange(item.getSalida());
                        item.clearSalida();
                    }
                }
                limite++;
                if (limite == 500)
                {
                    salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "Se ha superado el limite de iteraciones."));
                    return null;
                }
            } while ((bool)expresion.Ejecutar(ts));

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

        public Tipo getType()
        {
            return Tipo.WHILE;
        }

        public object Recolectar(TablaDeSimbolos ts)
        {
            return null;
        }
    }
}
