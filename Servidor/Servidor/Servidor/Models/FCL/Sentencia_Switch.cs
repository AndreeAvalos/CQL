using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models.FCL
{
    public class Sentencia_Switch : Instruccion
    {
        int line, column;
        List<string> salida = new List<string>();
        Tipo_Switch instruccion_switch;

        public Sentencia_Switch(Tipo_Switch instruccion_switch, int line, int column)
        {
            this.line = line;
            this.column = column;
            this.instruccion_switch = instruccion_switch;
        }

        public object Ejecutar(TablaDeSimbolos ts)
        {

            string opcion = instruccion_switch.Expresion.Ejecutar(ts).ToString();

            TablaDeSimbolos tabla_local = new TablaDeSimbolos();
            tabla_local.addPadre(ts);
            bool encontrado = false;
            foreach (Tipo_Case caso in instruccion_switch.Casos)
            {
                if (opcion.ToLower().Equals(caso.Expresion.ToLower())|| encontrado)
                {
                    encontrado = true;
                    foreach (Instruccion item in caso.Instrucciones)
                    {
                        if (item.getType() == Tipo.BREAK) return null ;
                        if (item.getType() == Tipo.USER_TYPES || item.getType() == Tipo.FUNCION || item.getType() == Tipo.METODO || item.getType() == Tipo.PROCEDURE)
                        {
                            salida.Add(Program.buildError(item.getLine(), item.getColumn(), "Semantico", "No puede venir instruccion del tipo: " + item.getType() + " en un ambito local."));
                        }
                        else
                        {
                            item.Ejecutar(tabla_local);
                            salida.AddRange(item.getSalida());
                        }
                    }
                }
            }
            foreach (Instruccion item in instruccion_switch.Instrucciones_default)
            {
                if (item.getType() == Tipo.BREAK) break;
                if (item.getType() == Tipo.USER_TYPES || item.getType() == Tipo.FUNCION || item.getType() == Tipo.METODO || item.getType() == Tipo.PROCEDURE)
                {
                    salida.Add(Program.buildError(item.getLine(), item.getColumn(), "Semantico", "No puede venir instruccion del tipo: " + item.getType() + " en un ambito local."));
                }
                else
                {
                    item.Ejecutar(tabla_local);
                    salida.AddRange(item.getSalida());
                }

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

        public Tipo getType()
        {
            return Tipo.SWITCH;
        }

        public object Recolectar(TablaDeSimbolos ts)
        {
            return null;
        }
    }
}
