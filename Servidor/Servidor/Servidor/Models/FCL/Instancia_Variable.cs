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
                Tipo type = ts.getType(new_var.Id);
                if (Program.casteos.comprobarCasteo(type, new_var.Valor))
                {
                    ts.setValor(new_var.Id, Program.casteos.castear(type, new_var.Valor));
                }
                else {
                    salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "No se puede convertir a "+ts.tipoAsignado(new_var.Id)));
                }

            }
            else salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", new_var.Id + " no esta declarada en este ambito."));
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
            return null;
        }
    }
}
