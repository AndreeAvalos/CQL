using Servidor.Models.CASTEOS;
using Servidor.NOSQL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models.FCL
{
    public class Asignacion_Variable : Instruccion
    {
        Tipo real_type;
        string type;
        bool global;
        List<Variable> variables_asignar;
        int line, column;
        List<string> salida = new List<string>();

        public Asignacion_Variable(Tipo real_type, string type, bool global, List<Variable> variables_asignar, int line, int column)
        {
            this.real_type = real_type;
            this.type = type;
            this.global = global;
            this.variables_asignar = variables_asignar;
            this.line = line;
            this.column = column;
        }

        public object Ejecutar(TablaDeSimbolos ts)
        {
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

            foreach (Variable item in variables_asignar)
            {
                if (!ts.existID(item.Id))
                {
                    Simbolo new_simbolo = new Simbolo(real_type, item.Id)
                    {
                        Tipo_asignado = item.Tipo,
                        Instanciado = item.Instanciada,
                        Sub_tipo = Tipo.VARIABLE
                    };

                    if (item.Instanciada)
                    {
                        Casteo casteos = new Casteo();
                        if (real_type == Tipo.ENTERO)
                        {
                            new_simbolo.Valor = casteos.castear(Tipo.ENTERO, item.Valor);
                        }
                    }
                    else {
                        if (real_type == Tipo.ENTERO)
                        {
                            new_simbolo.Valor = 0;
                        }

                    }
                    ts.AddLast(new_simbolo);
                } else salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", item.Id + " ObjectAlreadyExists"));
            }

            return ts;
        }
    }
}
