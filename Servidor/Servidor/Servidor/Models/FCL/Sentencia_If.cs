using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models.FCL
{
    public class Sentencia_If : Instruccion
    {
        int line, column;
        List<string> salida = new List<string>();
        Tipo_IF instruccion_if;

        public Sentencia_If(Tipo_IF instruccion_if, int line, int column)
        {
            this.line = line;
            this.column = column;
            this.instruccion_if = instruccion_if;
        }

        public object Ejecutar(TablaDeSimbolos ts)
        {
            TablaDeSimbolos tabla_local = new TablaDeSimbolos();
            tabla_local.addPadre(ts);
            if ((bool)instruccion_if.Condicion.Ejecutar(ts))
            {

                foreach (Instruccion item in instruccion_if.If_instrucciones)
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
            else
            {
                foreach (Tipo_IF item in instruccion_if.Else_if)
                {
                    if ((bool)item.Condicion.Ejecutar(ts))
                    {
                        foreach (Instruccion instruccion in item.If_instrucciones)
                        {
                            if (instruccion.getType() == Tipo.BREAK) break;
                            if (instruccion.getType() == Tipo.USER_TYPES || instruccion.getType()==Tipo.FUNCION|| instruccion.getType() == Tipo.METODO || instruccion.getType() == Tipo.PROCEDURE)
                            {
                                salida.Add(Program.buildError(instruccion.getLine(), instruccion.getColumn(), "Semantico", "No puede venir instruccion del tipo: " + instruccion.getType() + " en un ambito local."));
                            }
                            else
                            {
                                instruccion.Ejecutar(tabla_local);
                                salida.AddRange(instruccion.getSalida());
                            }
                        }
                        return null;
                    }
                }
                foreach (Instruccion instruccion in instruccion_if.Else_instrucciones)
                {
                    if (instruccion.getType() == Tipo.BREAK) break;
                    if (instruccion.getType() == Tipo.USER_TYPES || instruccion.getType() == Tipo.FUNCION || instruccion.getType() == Tipo.METODO || instruccion.getType() == Tipo.PROCEDURE)
                    {
                        salida.Add(Program.buildError(instruccion.getLine(), instruccion.getColumn(), "Semantico", instruccion.getType() + " solo es aceptada en un ambito global."));
                    }
                    else
                    {
                        instruccion.Ejecutar(tabla_local);
                        salida.AddRange(instruccion.getSalida());
                    }
                }
                return null;
            }
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
            return Tipo.IF;
        }

        public object Recolectar(TablaDeSimbolos ts)
        {
            return null;
        }
    }
}
