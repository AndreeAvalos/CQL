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
            if (!global)
            {
                foreach (Variable item in variables_asignar)
                {
                    if (!ts.existID_AA(item.Id.ToLower()))
                    {
                        Simbolo new_simbol = new Simbolo(real_type, item.Id);
                        new_simbol.Tipo_asignado = type;
                        if (item.Instanciada)
                        {
                            Operacion valor = (Operacion)item.Valor;
                            try
                            {
                                new_simbol.Valor = valor.Ejecutar(ts);
                                if (new_simbol.Valor == null) new_simbol.Valor = item.Valor;
                            }
                            catch (Exception)
                            {

                                new_simbol.Valor = valor;
                            }
                        }
                        else
                        {
                            if (real_type == Tipo.NUMERO) new_simbol.Valor = 0;
                            if (real_type == Tipo.OBJETO || real_type == Tipo.MAP || real_type == Tipo.LIST) new_simbol.Valor = null;

                        }
                        ts.AddLast(new_simbol);
                    }
                    else
                    {
                        salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", item.Id + " ObjectAlreadyExists3"));
                    }

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


        public object Recolectar(TablaDeSimbolos ts)
        {
            if (global)
            {
                foreach (Variable item in variables_asignar)
                {
                    if (!ts.existID_AA(item.Id.ToLower()))
                    {
                        Simbolo new_simbol = new Simbolo(real_type, item.Id);
                        new_simbol.Tipo_asignado = type;
                        if (item.Instanciada)
                        {
                            Operacion valor = (Operacion)item.Valor;
                            try
                            {
                                new_simbol.Valor = valor.Ejecutar(ts);
                                if (new_simbol.Valor == null) new_simbol.Valor = item.Valor;
                            }
                            catch (Exception)
                            {

                                new_simbol.Valor = valor;
                            }
                        }
                        else
                        {
                            if (real_type == Tipo.NUMERO) new_simbol.Valor = 0;
                            if (real_type == Tipo.OBJETO || real_type == Tipo.MAP || real_type == Tipo.LIST) new_simbol.Valor = null;

                        }
                        ts.AddLast(new_simbol);
                    }
                    else
                    {
                        salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", item.Id + " ObjectAlreadyExists4"));
                    }

                }
            }
            //else salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", item.Id + " ObjectAlreadyExists"));
            return null;

        }
    }
}
