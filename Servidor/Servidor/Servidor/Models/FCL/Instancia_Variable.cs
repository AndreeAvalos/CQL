using Servidor.NOSQL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models.FCL
{
    public class Instancia_Variable : Instruccion
    {
        int line, column;
        Variable new_var;
        List<string> salida = new List<string>();

        public Instancia_Variable(int line, int column, Variable new_var)
        {
            this.line = line;
            this.column = column;
            this.new_var = new_var;
        }

        public object Ejecutar(TablaDeSimbolos ts)
        {
            if (ts.existID(new_var.Id))
            {
                bool is_ok = true;
                foreach (string item in new_var.Lst_variables)
                {
                    if (!ts.existID(item.ToString()))
                    {
                        is_ok = false;
                        salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", item + " no esta declarada en este ambito."));
                    }
                }
                if (is_ok)
                {
                    Tipo type = ts.getType(new_var.Id);
                    Operacion val = (Operacion)new_var.Valor;
                    object valor = val.Ejecutar(ts);
                    salida.AddRange(val.getSalida());
                    if (valor != null)
                    {
                        if (Program.casteos.comprobarCasteo(type, valor))
                        {
                            ts.setValor(new_var.Id, valor);
                        }
                        else salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "No se puede convertir a" + ts.tipoAsignado(new_var.Id)));
                    }
                }
            }
            else salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", new_var.Id + " no esta declarada en este ambito."));
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
            return Tipo.VARIABLE;
        }

        public object Recolectar(TablaDeSimbolos ts)
        {
            return null;
        }
    }
}
