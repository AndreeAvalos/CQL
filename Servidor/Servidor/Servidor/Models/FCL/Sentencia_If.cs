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
                    item.Ejecutar(tabla_local);
                    salida.AddRange(item.getSalida());
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
                            instruccion.Ejecutar(tabla_local);
                            salida.AddRange(instruccion.getSalida());
                        }
                        return null;
                    }
                }
                foreach (Instruccion instruccion in instruccion_if.Else_instrucciones)
                {
                    instruccion.Ejecutar(tabla_local);
                    salida.AddRange(instruccion.getSalida());
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

        public object Recolectar(TablaDeSimbolos ts)
        {
            return null;
        }
    }
}
